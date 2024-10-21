using AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable;
using AzAIIntegration.Functions.Test;
using Azure.Messaging.ServiceBus;
using Csla;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AirCanada.Appx.AzAiIntegration.Functions.Test
{
    public partial class AiReceiptReaderResponseHandlerTests : IClassFixture<FunctionTestFixture>, IAsyncLifetime
    {
        public Mock<IConfiguration> _configurationMock;
        private readonly IServiceProvider _serviceProvider;

        public AiReceiptReaderResponseHandlerTests(FunctionTestFixture fixture)
        {
            _configurationMock = fixture.ConfigurationMock;
            _serviceProvider = fixture.ServiceProvider;
        }
        public Task InitializeAsync()
        {
            // Code to run before each test
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            // Code to run after each test
            using var scope = _serviceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;
            var context = scopedServiceProvider.GetRequiredService<AppxDbContext>();

            context.AzAiIntegrations.RemoveRange(context.AzAiIntegrations);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task ShouldLogAndThrow_WhenBrokenRules()
        {
            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;

            var dbContext = scopedServiceProvider.GetRequiredService<AppxDbContext>();
            DatabaseSeeder.SeedDatabase(dbContext);

            var mockMessageActions = new Mock<ServiceBusMessageActions>();
            var logger = scopedServiceProvider.GetRequiredService<ILogger<AiReceiptReaderResponseHandler>>();
            var portalFactory = scopedServiceProvider.GetRequiredService<IDataPortalFactory>();

            var handler = new AiReceiptReaderResponseHandler(_configurationMock.Object, portalFactory, logger);

            var aiReceiptReaderResponseMsg = MockResponseMessageHelper.CreateMockResponseMessage();
            aiReceiptReaderResponseMsg.MessageContext!.Environment = null; // Overwrite for this test scenario

            // Configure JsonSerializerOptions to use NodaTime converters and handle enums
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

            var messageBody = JsonSerializer.Serialize(aiReceiptReaderResponseMsg, options);
            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(
                            body: new BinaryData(Encoding.UTF8.GetBytes(messageBody)),
                            messageId: "test-message-id",
                            contentType: "application/json");

            // Act
            Func<Task> act = async () => await handler.Run(message, mockMessageActions.Object);

            // Assert
            var exception = await act.Should().ThrowAsync<Exception>();
            exception.WithInnerException<InvalidOperationException>()
                     .WithMessage("Exception@AiReceiptReaderResponseHandler => Broken rules: The ResponseMessageContext_Environment field is required.");
        }

        [Fact]
        public async Task ShouldLogAndThrow_WhenResponseMessageContext_HasErrorMessage()
        {
            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;

            var dbContext = scopedServiceProvider.GetRequiredService<AppxDbContext>();
            DatabaseSeeder.SeedDatabase(dbContext);

            var mockMessageActions = new Mock<ServiceBusMessageActions>();
            var logger = scopedServiceProvider.GetRequiredService<ILogger<AiReceiptReaderResponseHandler>>();
            var portalFactory = scopedServiceProvider.GetRequiredService<IDataPortalFactory>();

            var handler = new AiReceiptReaderResponseHandler(_configurationMock.Object, portalFactory, logger);

            var aiReceiptReaderResponseMsg = MockResponseMessageHelper.CreateMockResponseMessage();
            aiReceiptReaderResponseMsg.MessageContext!.Error = "AI Service Error: (InvalidRequest) Invalid request."; // Overwrite for this test scenario

            // Configure JsonSerializerOptions to use NodaTime converters and handle enums
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

            var messageBody = JsonSerializer.Serialize(aiReceiptReaderResponseMsg, options);
            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(
                            body: new BinaryData(Encoding.UTF8.GetBytes(messageBody)),
                            messageId: "test-message-id",
                            contentType: "application/json");

            // Act
            Func<Task> act = async () => await handler.Run(message, mockMessageActions.Object);

            // Assert
            var exception = await act.Should().ThrowAsync<Exception>();
            exception.WithInnerException<InvalidOperationException>()
                     .WithMessage("Exception@AiReceiptReaderResponseHandler => AI Service Error: AI Service Error: (InvalidRequest) Invalid request.");

            //Verify the database content
            var inMemoryDbContext = _serviceProvider.GetRequiredService<AppxDbContext>();

            var updatedEntity = await inMemoryDbContext.AzAiIntegrations.FirstOrDefaultAsync(e => e.Id == 98L);
            updatedEntity.Should().NotBeNull();
            updatedEntity!.ResponseMsg.Should().Be(messageBody);
            updatedEntity.Stage.Should().Be("Acknowledgement");
            updatedEntity.State.Should().Be("Failed");
            updatedEntity.ResponseId.Should().Be(aiReceiptReaderResponseMsg.MessageContext!.ResponseId);
            updatedEntity.UpdatedDateTime.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task ShouldHandleNodaTime_WhenDeserializingResponseMsg()
        {
            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;

            var dbContext = scopedServiceProvider.GetRequiredService<AppxDbContext>();
            DatabaseSeeder.SeedDatabase(dbContext);

            var mockMessageActions = new Mock<ServiceBusMessageActions>();
            var logger = scopedServiceProvider.GetRequiredService<ILogger<AiReceiptReaderResponseHandler>>();
            var portalFactory = scopedServiceProvider.GetRequiredService<IDataPortalFactory>();

            var handler = new AiReceiptReaderResponseHandler(_configurationMock.Object, portalFactory, logger);

            var messageBody = "{\"TotalAmount\": {\"InputContent\": \"250.00\", \"CalibrationType\": \"absolute\", \"CalibrationValue\": \"0.95\", \"ExtractedContent\": \"\", \"IsFound\": false, \"Confidence\": 0}, \"TransactionDate\": {\"InputContent\": \"0001-01-01\", \"ExtractedContent\": null, \"IsFound\": false, \"Confidence\": 0}, \"MessageContext\": {\"ResponseId\": null, \"CorrelationId\": 98, \"CreatedDateTime\": \"2024-10-08T13:55:29Z\", \"ModelVersion\": \"prebuilt-invoice\", \"Environment\": \"DEV\", \"Error\": null}}";
            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(
                            body: new BinaryData(Encoding.UTF8.GetBytes(messageBody)),
                            messageId: "3f5067ec-6f7e-4003-9f69-2302a1614c3a",
                            contentType: "application/json");

            // Act
            Func<Task> act = async () => await handler.Run(message, mockMessageActions.Object);

            // Assert
            var exception = await act.Should().ThrowAsync<InvalidOperationException>();
            exception.WithMessage("Exception@AiReceiptReaderResponseHandler => An error occurred while processing the Receipt Reader response message Id 3f5067ec-6f7e-4003-9f69-2302a1614c3a.")
                     .WithInnerException<InvalidOperationException>()
                     .WithMessage("Exception@AiReceiptReaderResponseHandler => Broken rules: The ResponseMessageContext_Responseid field is required.");
        }
    }
}
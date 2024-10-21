using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals;
using AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.Test.ReceiptReader.Dals
{
    public class ReceiptReaderResponseDalTests : IClassFixture<DalTestFixture>, IAsyncLifetime
    {
        private readonly IServiceProvider _serviceProvider;

        public ReceiptReaderResponseDalTests(DalTestFixture fixture)
        {
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
        public void Fetch_ShouldThrowArgumentNullException_WhenRequestIdIsInvalid()
        {
            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;

            var context = scopedServiceProvider.GetRequiredService<AppxDbContext>();
            var dal = scopedServiceProvider.GetRequiredService<IReceiptReaderResponseDal>();
            long invalidRequestId = 0;

            // Act
            void Act() => dal.Fetch(invalidRequestId);
            Action act = Act;

            // Assert
            var exception = Assert.Throws<InvalidOperationException>(act);
            exception.Message.Should().Contain($"Invalid request ID: {invalidRequestId}. Request ID must be greater than zero.");

            // Check inner exception details
            exception.InnerException.Should().NotBeNull();
            exception.InnerException.Should().BeOfType<ArgumentNullException>();
            exception.InnerException!.Message.Should().Contain($"Invalid request ID: {invalidRequestId}. Request ID must be greater than zero.");
        }

        [Fact]
        public void Fetch_ShouldReturnNull_WhenRecordDoesNotExist()
        {
            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;

            var context = scopedServiceProvider.GetRequiredService<AppxDbContext>();
            var dal = scopedServiceProvider.GetRequiredService<IReceiptReaderResponseDal>();

            // Act
            var result = dal.Fetch(long.MaxValue);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Fetch_ShouldReturnCorrectValues_WhenRequestRecordExists()
        {
            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;

            var context = scopedServiceProvider.GetRequiredService<AppxDbContext>();
            DatabaseSeeder.SeedDatabase(context);

            var dal = scopedServiceProvider.GetRequiredService<IReceiptReaderResponseDal>();

            // Assuming the seeded database has a record with Id = 1
            long requestId = 98L;

            // Act
            var result = dal.Fetch(requestId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(requestId);
            result.AiResponseMessage.Should().BeNullOrEmpty();
            result.MessageContext.Should().NotBeNull();
            result.MessageContext!.Responseid.Should().BeNull();
            result.MessageContext.CreatedDateTime.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
            result.MessageContext.Environment.Should().BeNull();
            result.MessageContext.Error.Should().BeNull();
        }

        [Fact]
        public void Update_ShouldSaveResponseMessageAndUpdateFields()
        {
            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;

            var context = scopedServiceProvider.GetRequiredService<AppxDbContext>();
            DatabaseSeeder.SeedDatabase(context);

            var logger = scopedServiceProvider.GetRequiredService<ILogger<ReceiptReaderResponseDal>>();
            var dal = scopedServiceProvider.GetRequiredService<IReceiptReaderResponseDal>();
            var mockDto = MockDtoHelper.CreateMockResponseDto();

            // Act
            dal.Update(mockDto);

            // Assert
            var updatedEntity = context.AzAiIntegrations.Find(mockDto.Id);
            updatedEntity.Should().NotBeNull();
            updatedEntity!.ResponseMsg.Should().Contain(mockDto.AiResponseMessage);
            updatedEntity.ResponseId.Should().Be(mockDto.MessageContext!.Responseid);
            updatedEntity.UpdatedDateTime.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        }
    }
}
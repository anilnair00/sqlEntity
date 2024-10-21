using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals;
using AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable;
using AutoMapper;
using Csla;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AirCanada.Appx.AzAiIntegration.Test
{
    public class ReceiptReaderResponseEditTests : IClassFixture<DiCslaFixture>, IAsyncLifetime
    {
        private readonly IServiceProvider _serviceProvider;

        public ReceiptReaderResponseEditTests(DiCslaFixture fixture)
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
        public void Fetch_ShouldNotPopulateProperties_WhenRecordIsNotFound()
        {
            // Arrange
            long requestId = long.MaxValue;

            // Act
            var dpFactory = _serviceProvider.GetRequiredService<IDataPortalFactory>();
            var receiptReaderResponseEdit = dpFactory.GetPortal<ReceiptReaderResponseEdit>().Fetch(requestId);

            // Assert
            receiptReaderResponseEdit.ResponseMessageContext_Responseid.Should().BeNull();
            receiptReaderResponseEdit.ResponseMessageContext_CorrelationId.Should().BeNull();
            receiptReaderResponseEdit.ResponseMessageContext_ModelVersion.Should().Be(string.Empty);
            receiptReaderResponseEdit.ResponseMessageContext_CreatedDateTime.Should().BeNull();
            receiptReaderResponseEdit.ResponseMessageContext_Environment.Should().BeNull();
            receiptReaderResponseEdit.ResponseMessageContext_Error.Should().Be(string.Empty);
            receiptReaderResponseEdit.TotalAmount_InputContent.Should().Be(string.Empty);
            receiptReaderResponseEdit.TotalAmount_CalibrationType.Should().Be(string.Empty);
            receiptReaderResponseEdit.TotalAmount_CalibrationValue.Should().Be(string.Empty);
            receiptReaderResponseEdit.TotalAmount_ExtractedContent.Should().Be(string.Empty);
            receiptReaderResponseEdit.TotalAmount_IsFound.Should().BeNull();
            receiptReaderResponseEdit.TotalAmount_Confidence.Should().BeNull();
            receiptReaderResponseEdit.TransactionDate_InputContent.Should().BeNull();
            receiptReaderResponseEdit.TransactionDate_ExtractedContent.Should().BeNull();
            receiptReaderResponseEdit.TransactionDate_IsFound.Should().BeNull();
            receiptReaderResponseEdit.TransactionDate_Confidence.Should().BeNull();

            // Check for broken rules
            var brokenRules = receiptReaderResponseEdit.BrokenRulesCollection;
            brokenRules.Should().Contain(br => br.Property == nameof(receiptReaderResponseEdit.ResponseMessageContext_Responseid));
            brokenRules.Should().Contain(br => br.Property == nameof(receiptReaderResponseEdit.ResponseMessageContext_CorrelationId));
            brokenRules.Should().Contain(br => br.Property == nameof(receiptReaderResponseEdit.ResponseMessageContext_Environment));
            brokenRules.Should().Contain(br => br.Property == nameof(receiptReaderResponseEdit.TotalAmount_InputContent));
            brokenRules.Should().Contain(br => br.Property == nameof(receiptReaderResponseEdit.TotalAmount_IsFound));
            brokenRules.Should().Contain(br => br.Property == nameof(receiptReaderResponseEdit.TransactionDate_InputContent));
        }

        [Fact]
        public async Task Update_ShouldSaveResponseMessageAndUpdateFields()
        {
            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;

            var context = scopedServiceProvider.GetRequiredService<AppxDbContext>();
            DatabaseSeeder.SeedDatabase(context);

            var mapper = scopedServiceProvider.GetRequiredService<IMapper>();
            var logger = scopedServiceProvider.GetRequiredService<ILogger<ReceiptReaderResponseEdit>>();
            var dal = scopedServiceProvider.GetRequiredService<IReceiptReaderResponseDal>();
            var dpFactory = scopedServiceProvider.GetRequiredService<IDataPortalFactory>();

            // Fetch the existing record
            long requestId = 98L;
            var receiptReaderResponseEdit = dpFactory.GetPortal<ReceiptReaderResponseEdit>().Fetch(requestId);

            // Update properties using helper
            ReceiptReaderResponseEditHelper.MockInstance(receiptReaderResponseEdit);

            // Act
            var receiptReaderResponseEdited = await receiptReaderResponseEdit.SaveAsync();

            // Assert
            var inMemoryDbContext = _serviceProvider.GetRequiredService<AppxDbContext>();

            var updatedEntity = inMemoryDbContext.AzAiIntegrations.Find(receiptReaderResponseEdited.Id);
            updatedEntity.Should().NotBeNull();
            updatedEntity!.ResponseMsg.Should().Contain(receiptReaderResponseEdited.AiResponseMessage);
            updatedEntity.ResponseId.Should().Be(receiptReaderResponseEdited.ResponseMessageContext_Responseid);
            updatedEntity.UpdatedDateTime.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        }
    }
}
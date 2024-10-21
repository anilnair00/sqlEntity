using AirCanada.Appx.Claim.Expense;
using Csla;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace AirCanada.Appx.AzAiIntegration.IntegrationTest
{
    public class ExpenseReceiptDocumentInfoIntegrationTests : IClassFixture<AzAiIntegrationTestFixture>
    {
        private readonly IServiceProvider _serviceProvider;

        public ExpenseReceiptDocumentInfoIntegrationTests(AzAiIntegrationTestFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        // The main behavior of this business object is to fetch the Expense Receipt Document Information along with all the properties
        // required to initialize the payload of the AI service request message.
        // More precisely, the Ids of the parent entities such as the SsetOperations and SsetOperationExpenses.

        // [Fact]
        public async Task Fetch_ExpenseReceiptDocument_ReturnsData()
        {
            var dpFactory = _serviceProvider.GetRequiredService<IDataPortalFactory>();
            var annotationId = new Guid("D643D852-3C27-EF11-840B-000D3A99B674");

            // Fetch the ExpenseReceiptDocumentInfo object
            var expenseReceiptDocumentInfo = await dpFactory.GetPortal<ExpenseReceiptDocumentInfo>().FetchAsync(annotationId);

            expenseReceiptDocumentInfo.Should().NotBeNull();
            expenseReceiptDocumentInfo.FileName.Should().Be("fake_receipt.png");
        }
    }
}

using AirCanada.Appx.Claim.Expense;
using Csla;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AirCanada.Appx.AzAiIntegration.SystemTest.Claim
{
    public class ExpenseReceiptDocumentEditTests : IClassFixture<SystemTestBoFixture>
    {
        private readonly IServiceProvider _serviceProvider;

        public ExpenseReceiptDocumentEditTests(SystemTestBoFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        [Fact]
        public async Task ShouldFetchExpenseReceiptDocumentEdit_WhenFetchIsCalled()
        {
            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;
            var dataPortal = scopedServiceProvider.GetRequiredService<IDataPortal<ExpenseReceiptDocumentEdit>>();

            // Act
            var expenseReceiptDocumentEdit = await dataPortal.FetchAsync(95L);

            // Assert
            expenseReceiptDocumentEdit.Should().NotBeNull();
            expenseReceiptDocumentEdit.Id.Should().Be(95L);
            expenseReceiptDocumentEdit.ExtractedAmount.Should().Be(44.43m);
            expenseReceiptDocumentEdit.IsValidAmount.Should().BeFalse();
            expenseReceiptDocumentEdit.IsValidDate.Should().BeFalse();
            expenseReceiptDocumentEdit.ExtractedDate.Should().BeCloseTo(DateTime.Parse("2020-08-12"), TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task ShouldUpdateExpenseReceiptDocumentEdit_WhenUpdateIsCalled()
        {
            long requestId = 96L;

            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;
            var dataPortal = scopedServiceProvider.GetRequiredService<IDataPortal<ExpenseReceiptDocumentEdit>>();

            // Fetch the original record
            var originalExpenseReceiptDocumentEdit = await dataPortal.FetchAsync(requestId);
            originalExpenseReceiptDocumentEdit.Should().NotBeNull();

            // Modify the business object for the update
            originalExpenseReceiptDocumentEdit.ExtractedAmount += 1; // Change the amount
            originalExpenseReceiptDocumentEdit.IsValidAmount = !originalExpenseReceiptDocumentEdit.IsValidAmount; // Toggle the validity
            originalExpenseReceiptDocumentEdit.IsValidDate = !originalExpenseReceiptDocumentEdit.IsValidDate; // Toggle the validity
            originalExpenseReceiptDocumentEdit.ExtractedDate = originalExpenseReceiptDocumentEdit.ExtractedDate?.AddDays(1); // Change the date

            // Act
            var updatedExpenseReceiptDocumentEdit = await originalExpenseReceiptDocumentEdit.SaveAsync();

            // Fetch the updated record
            var fetchedUpdatedExpenseReceiptDocumentEdit = await dataPortal.FetchAsync(requestId);

            // Assert
            fetchedUpdatedExpenseReceiptDocumentEdit.Should().NotBeNull();
            fetchedUpdatedExpenseReceiptDocumentEdit.Id.Should().Be(updatedExpenseReceiptDocumentEdit.Id);
            fetchedUpdatedExpenseReceiptDocumentEdit.ExtractedAmount.Should().Be(updatedExpenseReceiptDocumentEdit.ExtractedAmount);
            fetchedUpdatedExpenseReceiptDocumentEdit.IsValidAmount.Should().Be(updatedExpenseReceiptDocumentEdit.IsValidAmount);
            fetchedUpdatedExpenseReceiptDocumentEdit.IsValidDate.Should().Be(updatedExpenseReceiptDocumentEdit.IsValidDate);
            fetchedUpdatedExpenseReceiptDocumentEdit.ExtractedDate.Should().BeCloseTo(updatedExpenseReceiptDocumentEdit.ExtractedDate!.Value, TimeSpan.FromSeconds(1));

            // Restore the original record
            updatedExpenseReceiptDocumentEdit.ExtractedAmount -= 1; // Revert the amount
            updatedExpenseReceiptDocumentEdit.IsValidAmount = !updatedExpenseReceiptDocumentEdit.IsValidAmount; // Revert the validity
            updatedExpenseReceiptDocumentEdit.IsValidDate = !updatedExpenseReceiptDocumentEdit.IsValidDate; // Revert the validity
            updatedExpenseReceiptDocumentEdit.ExtractedDate = updatedExpenseReceiptDocumentEdit.ExtractedDate?.AddDays(-1); // Revert the date

            // Act
            var restoredExpenseReceiptDocumentEdit = await updatedExpenseReceiptDocumentEdit.SaveAsync();

            // Fetch the restored record
            var fetchedRestoredExpenseReceiptDocumentEdit = await dataPortal.FetchAsync(requestId);

            // Assert restoration
            fetchedRestoredExpenseReceiptDocumentEdit.Should().NotBeNull();
            fetchedRestoredExpenseReceiptDocumentEdit.Id.Should().Be(restoredExpenseReceiptDocumentEdit.Id);
            fetchedRestoredExpenseReceiptDocumentEdit.ExtractedAmount.Should().Be(restoredExpenseReceiptDocumentEdit.ExtractedAmount);
            fetchedRestoredExpenseReceiptDocumentEdit.IsValidAmount.Should().Be(restoredExpenseReceiptDocumentEdit.IsValidAmount);
            fetchedRestoredExpenseReceiptDocumentEdit.IsValidDate.Should().Be(restoredExpenseReceiptDocumentEdit.IsValidDate);
            fetchedRestoredExpenseReceiptDocumentEdit.ExtractedDate.Should().BeCloseTo(restoredExpenseReceiptDocumentEdit.ExtractedDate!.Value, TimeSpan.FromSeconds(1));
        }
    }
}
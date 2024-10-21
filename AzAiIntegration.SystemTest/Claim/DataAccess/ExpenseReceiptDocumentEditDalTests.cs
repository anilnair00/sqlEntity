using AirCanada.Appx.Claim.DataAccess.Expense.Dals;
using AirCanada.Appx.Claim.DataAccess.Expense.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;

namespace AirCanada.Appx.AzAiIntegration.SystemTest.Claim.DataAccess
{
    public class ExpenseReceiptDocumentEditDalTests : IClassFixture<SystemTestFixture>
    {
        private readonly IServiceProvider _serviceProvider;

        public ExpenseReceiptDocumentEditDalTests(SystemTestFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        [Fact]
        public void ShouldReturnExpectedDto_WhenFetchIsCalled()
        {
            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;
            var dal = scopedServiceProvider.GetRequiredService<IExpenseReceiptDocumentEditDal>();

            // Act
            var dto = dal.Fetch(95L); 

            // Assert
            dto.Should().NotBeNull();
            dto!.Id.Should().Be(95L);
            dto.ExtractedAmount.Should().Be(44.43m);
            dto.IsValidAmount.Should().BeFalse();
            dto.IsValidDate.Should().BeFalse();
            dto.ExtractedDate.Should().BeCloseTo(DateTime.Parse("2020-08-12"), TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void ShouldUpdateRecord_WhenUpdateIsCalled()
        {
            long requestId = 84L;

            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;
            var dal = scopedServiceProvider.GetRequiredService<IExpenseReceiptDocumentEditDal>();

            // Fetch the original record
            var originalDto = dal.Fetch(requestId);
            originalDto.Should().NotBeNull();

            // Modify the DTO for the update
            var updatedDto = new ExpenseReceiptDocumentEditDto
            {
                Id = originalDto!.Id,
                ExtractedAmount = originalDto.ExtractedAmount + 1, // Change the amount
                IsValidAmount = !originalDto.IsValidAmount, // Toggle the validity
                IsValidDate = !originalDto.IsValidDate, // Toggle the validity
                ExtractedDate = originalDto.ExtractedDate?.AddDays(1) // Change the date
            };

            // Act
            dal.Update(updatedDto);

            // Fetch the updated record
            var fetchedDto = dal.Fetch(requestId);

            // Assert
            fetchedDto.Should().NotBeNull();
            fetchedDto!.Id.Should().Be(updatedDto.Id);
            fetchedDto.ExtractedAmount.Should().Be(updatedDto.ExtractedAmount);
            fetchedDto.IsValidAmount.Should().Be(updatedDto.IsValidAmount);
            fetchedDto.IsValidDate.Should().Be(updatedDto.IsValidDate);
            fetchedDto.ExtractedDate.Should().BeCloseTo(updatedDto.ExtractedDate!.Value, TimeSpan.FromSeconds(1));

            // Restore the original record
            dal.Update(originalDto);

            // Fetch the restored record
            var restoredDto = dal.Fetch(requestId);

            // Assert restoration
            restoredDto.Should().NotBeNull();
            restoredDto!.Id.Should().Be(originalDto.Id);
            restoredDto.ExtractedAmount.Should().Be(originalDto.ExtractedAmount);
            restoredDto.IsValidAmount.Should().Be(originalDto.IsValidAmount);
            restoredDto.IsValidDate.Should().Be(originalDto.IsValidDate);
            restoredDto.ExtractedDate.Should().BeCloseTo(originalDto.ExtractedDate!.Value, TimeSpan.FromSeconds(1));
        }
    }
}
using AirCanada.Appx.Claim.Expense;
using Csla;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AirCanada.Appx.AzAiIntegration.IntegrationTest
{
    public class ExpenseReceiptDocumentEditTests : IClassFixture<AzAiIntegrationTestFixture>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;

        public ExpenseReceiptDocumentEditTests(AzAiIntegrationTestFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
            _config = fixture.Configuration;
        }

        //[Fact]
        public async Task FetchValidObject_ShouldSucceed()
        {
            //Arrange
            var dpFactory = _serviceProvider.GetRequiredService<IDataPortalFactory>();
            var validId = 103L;

            //Act
            var expenseEdit = await dpFactory.GetPortal<ExpenseReceiptDocumentEdit>().FetchAsync(validId);

            //Assert
            Assert.NotNull(expenseEdit);
        }

        //[Fact]
        public async Task FetchInvalidObject_ShouldFail()
        {
            //Arrange
            var dpFactory = _serviceProvider.GetRequiredService<IDataPortalFactory>();
            var invalidId = 107L;

            //Act
            var exception = await Record.ExceptionAsync(() => dpFactory.GetPortal<ExpenseReceiptDocumentEdit>().FetchAsync(invalidId));

            //Assert
            Assert.NotNull(exception);
            exception.Message.Should().Be("DataPortal.Fetch failed (Invalid transaction. Record does not exist or is not valid.)");
        }

        //[Fact]
        public async Task InsertValidObject_ShouldSucceed()
        {
            //Arrange
            var dpFactory = _serviceProvider.GetRequiredService<IDataPortalFactory>();
            var expenseEdit = await dpFactory.GetPortal<ExpenseReceiptDocumentEdit>().FetchAsync(101L);
            expenseEdit.IsValidAmount = false;
            expenseEdit.IsValidDate = true;
            expenseEdit.ExtractedAmount = 25;
            expenseEdit.ExtractedDate = DateTime.Now;

            //Act
            var result = await expenseEdit.SaveAsync();

            //Assert
            expenseEdit.IsValid.Should().BeTrue();
        }

        //[Fact]
        public async Task InsertInvalidObject_ShouldFail()
        {
            //Arrange
            var dpFactory = _serviceProvider.GetRequiredService<IDataPortalFactory>();
            var expenseEdit = await dpFactory.GetPortal<ExpenseReceiptDocumentEdit>().FetchAsync(103L);

            //Act
            var exception = await Record.ExceptionAsync(() => expenseEdit.SaveAsync());

            //Assert
            expenseEdit.IsValid.Should().BeFalse();
            exception.Message.Should().Be("Object is not valid and can not be saved");
        }
    }
}

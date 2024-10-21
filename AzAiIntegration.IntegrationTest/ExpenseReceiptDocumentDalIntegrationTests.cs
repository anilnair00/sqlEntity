//using AirCanada.Appx.Claim.DataAccess.Expense;
//using AirCanada.Appx.Claim.DataAccess.Expense.Dals;
//using AirCanada.Appx.Common.Wrappers;
//using AzAiIntegration.IntegrationTest.Setup;
//using FluentAssertions;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AirCanada.Appx.AzAiIntegration.IntegrationTest
//{
//    public class ExpenseReceiptDocumentDalIntegrationTests
//    {
//        private readonly ExpenseReceiptDocumentDal _expenseReceiptDocumentDal;
//        private IConfiguration _config;

//        public ExpenseReceiptDocumentDalIntegrationTests(IntegrationTestFixture fixture)
//        {
//            _config = fixture.Configuration;
//            _expenseReceiptDocumentDal = new ExpenseReceiptDocumentDal(_config);
//        }

//        [Fact]
//        public async Task Fetch_ShouldReturnReceiptDocument()
//        {
//            // Arrange
//            Guid annotationId = new Guid("D643D852-3C27-EF11-840B-000D3A99B674");

//            // Act
//            var expenseReceiptDocumentDto = await _expenseReceiptDocumentDal.Fetch(annotationId);

//            // Assert
//            expenseReceiptDocumentDto.Should().NotBeNull();
//            expenseReceiptDocumentDto!.OperationDocument.DynamicsAnnotationWebRequestId.Should().Be(annotationId);
//            expenseReceiptDocumentDto.OperationDocument.FileName.Should().Be("fake_receipt.png");
//        }
//    }
//}

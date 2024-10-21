using AirCanada.Appx.Claim.DataAccess.Expense.Dals;
using AirCanada.Appx.Claim.DataAccess.Expense.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using Moq;
using System.Data;
using Dapper;
using Xunit.Abstractions;

namespace AirCanada.Appx.Claim.DataAccess.Test.Expense.Dals
{
    public class ExpenseReceiptDocumentEditDalTests : IClassFixture<DalTestFixture>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITestOutputHelper _output;

        public ExpenseReceiptDocumentEditDalTests(DalTestFixture fixture, ITestOutputHelper output)
        {
            _serviceProvider = fixture.ServiceProvider;
            _output = output;
        }

        [Fact]
        public void ShouldReturnExpectedDto_WhenFetchIsCalled()
        {
            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;
            var dal = scopedServiceProvider.GetRequiredService<IExpenseReceiptDocumentEditDal>();
            var dbConnectionMock = scopedServiceProvider.GetRequiredService<Mock<IDbConnection>>();
            var dapperWrapperMock = scopedServiceProvider.GetRequiredService<Mock<IDapperWrapper>>();

            // Setup mock behavior
            var expectedDto = new ExpenseReceiptDocumentEditDto
            {
                Id = 95L,
                ExtractedAmount = 43.43m,
                IsValidAmount = true,
                IsValidDate = true,
                ExtractedDate = DateTime.Parse("2020-08-11")
            };

            // Mock Dapper's QueryFirstOrDefault method
            dapperWrapperMock.Setup(dw => dw.QueryFirstOrDefault<ExpenseReceiptDocumentEditDto>(dbConnectionMock.Object, It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                             .Returns(expectedDto);

            // Act
            var dto = dal.Fetch(95L);

            // Assert
            dto.Should().NotBeNull();
            dto!.Id.Should().Be(95L);
            dto.ExtractedAmount.Should().Be(43.43m);
            dto.IsValidAmount.Should().BeTrue();
            dto.IsValidDate.Should().BeTrue();
            dto.ExtractedDate.Should().BeCloseTo(DateTime.Parse("2020-08-11"), TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void ShouldCallExecute_WhenUpdateIsCalled()
        {
            // Arrange
            using var scope = _serviceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;
            var dal = scopedServiceProvider.GetRequiredService<IExpenseReceiptDocumentEditDal>();
            var dbConnectionMock = scopedServiceProvider.GetRequiredService<Mock<IDbConnection>>();
            var dapperWrapperMock = scopedServiceProvider.GetRequiredService<Mock<IDapperWrapper>>();

            var dto = new ExpenseReceiptDocumentEditDto
            {
                Id = 95L,
                ExtractedAmount = 42.43m,
                IsValidAmount = false,
                IsValidDate = false,
                ExtractedDate = DateTime.Parse("2020-08-10")
            };

            object? capturedParams = null;

            dapperWrapperMock.Setup(dw => dw.Execute(
                It.IsAny<IDbConnection>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
                .Callback<IDbConnection, string, object, IDbTransaction, int?, CommandType?>((conn, sql, param, trans, timeout, type) =>
                {
                    capturedParams = param;
                });

            // Act
            dal.Update(dto);

            // Assert
            dapperWrapperMock.Verify(dw => dw.Execute(
                dbConnectionMock.Object,
                It.Is<string>(sql => sql.Contains("UPDATE [Claim].[SsetOperationDocuments]")),
                It.IsAny<object>(),
                null, null, null), Times.Once);

            // Verify parameters
            capturedParams.Should().NotBeNull();
            VerifyParameters(capturedParams!, dto).Should().BeTrue();
        }

        private bool VerifyParameters(object param, ExpenseReceiptDocumentEditDto dto)
        {
            IDictionary<string, object> paramDict;

            if (param is IDictionary<string, object> dictionary)
            {
                paramDict = dictionary;
            }
            else
            {
                // Convert anonymous type to dictionary with non-nullable values
                paramDict = param.GetType()
                                 .GetProperties()
                                 .ToDictionary(
                                     prop => prop.Name,
                                     prop => prop.GetValue(param, null) ?? throw new InvalidOperationException($"Property {prop.Name} is null")
                                 );
            }

            _output.WriteLine($"Id: {paramDict["Id"]}, Expected: {dto.Id}");
            _output.WriteLine($"ExtractedAmount: {paramDict["ExtractedAmount"]}, Expected: {dto.ExtractedAmount}");
            _output.WriteLine($"IsValidAmount: {paramDict["IsValidAmount"]}, Expected: {dto.IsValidAmount}");
            _output.WriteLine($"IsValidDate: {paramDict["IsValidDate"]}, Expected: {dto.IsValidDate}");
            _output.WriteLine($"ExtractedDate: {paramDict["ExtractedDate"]}, Expected: {dto.ExtractedDate}");

            return paramDict.TryGetValue("Id", out var id) && id is long idValue && idValue == dto.Id &&
                   paramDict.TryGetValue("ExtractedAmount", out var extractedAmount) && extractedAmount is decimal extractedAmountValue && extractedAmountValue == dto.ExtractedAmount &&
                   paramDict.TryGetValue("IsValidAmount", out var isValidAmount) && isValidAmount is bool isValidAmountValue && isValidAmountValue == dto.IsValidAmount &&
                   paramDict.TryGetValue("IsValidDate", out var isValidDate) && isValidDate is bool isValidDateValue && isValidDateValue == dto.IsValidDate &&
                   paramDict.TryGetValue("ExtractedDate", out var extractedDate) && extractedDate is DateTime extractedDateValue && extractedDateValue == dto.ExtractedDate;
        }
    }
}
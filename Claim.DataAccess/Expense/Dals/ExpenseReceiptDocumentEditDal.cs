using AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable;
using AirCanada.Appx.Claim.DataAccess.Expense.Dtos;
using AirCanada.Appx.Common.Extensions;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace AirCanada.Appx.Claim.DataAccess.Expense.Dals
{
    public class ExpenseReceiptDocumentEditDal : IExpenseReceiptDocumentEditDal
    {
        private readonly IDapperWrapper _dapperWrapper;
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<ExpenseReceiptDocumentEditDal> _logger;

        public ExpenseReceiptDocumentEditDal(IDapperWrapper dapperWrapper, IDbConnection dbConnection, ILogger<ExpenseReceiptDocumentEditDal> logger)
        {
            _dapperWrapper = dapperWrapper;
            _dbConnection = dbConnection;
            _logger = logger;
        }

        public ExpenseReceiptDocumentEditDto? Fetch(long ssetOperationDocumentsId)
        {
            if (ssetOperationDocumentsId <= 0)
            {
                var errorMsg = $"Invalid parameter value on Fetch {nameof(ssetOperationDocumentsId)}: {ssetOperationDocumentsId}";
                _logger.LogAndThrow(nameof(ExpenseReceiptDocumentEditDal), errorMsg, new ArgumentNullException(nameof(ssetOperationDocumentsId)));
            }
            
            var sql = @"SELECT Id, ExtractedAmount, IsValidAmount, ExtractedDate, IsValidDate, SsetOperationExpenseId
                        FROM [Claim].[SsetOperationDocuments]
                        WHERE Id = @Id";

            try
            {
                var param = new { Id = ssetOperationDocumentsId };
                var result = _dapperWrapper.QueryFirstOrDefault<ExpenseReceiptDocumentEditDto>(_dbConnection, sql, param);

                if (result is null || result.Id <= 0)
                {
                    var errorMsg = $"Referential integrity issue: Record with SsetOperationDocumentsId {ssetOperationDocumentsId} does not exist.";
                    _logger.LogAndThrow(nameof(ExpenseReceiptDocumentEditDal), errorMsg);
                }

                return new ExpenseReceiptDocumentEditDto
                {
                    Id = result!.Id,
                    ExtractedAmount = result.ExtractedAmount,
                    IsValidAmount = result.IsValidAmount,
                    ExtractedDate = result.ExtractedDate,
                    IsValidDate = result.IsValidDate
                };
            }
            catch (Exception ex)
            {
                var errorMsg = $"An error occurred while fetching record with SsetOperationDocumentsId {ssetOperationDocumentsId}.";
                _logger.LogAndThrow(nameof(ExpenseReceiptDocumentEditDal), errorMsg, ex);
            }

            return null;
        }

        public void Update(ExpenseReceiptDocumentEditDto dto)
        {
            var sql = @"UPDATE [Claim].[SsetOperationDocuments] 
                        SET ExtractedAmount = @ExtractedAmount,
                            isValidAmount = @isValidAmount,
                            isValidDate = @isValidDate,
                            ExtractedDate = @ExtractedDate
                        WHERE Id = @Id";

            try
            {
                var parameters = new
                {
                    dto.ExtractedAmount,
                    dto.IsValidAmount,
                    dto.IsValidDate,
                    dto.ExtractedDate,
                    dto.Id
                };

                _dapperWrapper.Execute(_dbConnection, sql, parameters);
            }
            catch (Exception ex)
            {
                var errorMsg = $"An error occurred while updating record with Id {dto.Id}.";
                _logger.LogAndThrow(nameof(ExpenseReceiptDocumentEditDal), errorMsg, ex);
                throw; // Re-throw the exception to ensure the caller is aware of the failure
            }
        }
    }
}
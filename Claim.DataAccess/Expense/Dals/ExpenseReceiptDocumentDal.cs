using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Models;
using AirCanada.Appx.Claim.DataAccess.Expense.Dtos;
using AirCanada.Appx.Claim.DataAccess.Expense.Models;
using AirCanada.Appx.Common.Extensions;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace AirCanada.Appx.Claim.DataAccess.Expense.Dals
{
    public class ExpenseReceiptDocumentDal : IExpenseReceiptDocumentDal
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ExpenseReceiptDocumentDal> _logger;

        public ExpenseReceiptDocumentDal(IConfiguration configuration, ILogger<ExpenseReceiptDocumentDal> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public ExpenseReceiptDocumentDto? Fetch(Guid annotationId, ILogger<IExpenseReceiptDocumentDal> logger)
        {
            var expenseReceiptDocumentDto = new ExpenseReceiptDocumentDto();

            expenseReceiptDocumentDto = QueryExpenseTables(annotationId, logger);

            return expenseReceiptDocumentDto;
        }

        public ExpenseReceiptDocumentDto? QueryExpenseTables(Guid annotationId, ILogger<IExpenseReceiptDocumentDal> logger)
        {
            ExpenseReceiptDocumentDto? expenseReceiptDocumentDto = new();

            var connectionString = _configuration["APPX-Claim-ConnectionString"];

            // Validate the connection string
            ValidateSqlConnectionString(connectionString);

            var sql = @"SELECT d.*, e.Id as ExpenseId, e.*, o.Id as OperationId, o.*
                            FROM [Claim].[SsetOperationDocuments] d
                            JOIN [Claim].[SsetOperationExpenses] e ON d.[SsetOperationExpenseId] = e.Id
                            JOIN [Claim].[SsetOperations] o ON e.[SsetOperationId] = o.Id
                            WHERE d.DynamicsAnnotationWebRequestId = @Id";

            var currencySql = @"SELECT *
                                    FROM [MasterData].[Currency]
                                    WHERE Id = @Id";

            var languageSql = @"SELECT Code
                                    FROM [MasterData].[Language]
                                    WHERE Id = @Id";

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                IDbConnection idbConnection = sqlConnection;

                try
                {
                    idbConnection.Open();

                    expenseReceiptDocumentDto = idbConnection.Query<SsetOperationDocumentEntity, SsetOperationExpenseEntity, SsetOperationEntity, ExpenseReceiptDocumentDto>(
                        sql,
                        (document, expense, operation) =>
                        {
                            var dto = new ExpenseReceiptDocumentDto();
                            dto.OperationDocument = document;
                            dto.OperationExpense = expense;
                            dto.Operation = operation;
                            return dto;
                        },
                        new { Id = annotationId },
                        splitOn: "ExpenseId,OperationId"
                    ).FirstOrDefault();

                    if (expenseReceiptDocumentDto != null)
                    {

                        var currencyInfo = idbConnection.QueryFirstOrDefault<CurrencyModel>(
                            currencySql,
                            new { Id = expenseReceiptDocumentDto.OperationExpense.CurrencyId }
                        );

                        if (currencyInfo != null)
                        {
                            expenseReceiptDocumentDto.OperationExpense.CurrencyCode = currencyInfo.Code;
                            expenseReceiptDocumentDto.OperationExpense.CurrencySymbol = currencyInfo.Symbol;
                        }

                        var languageInfo = idbConnection.QueryFirstOrDefault<string>(
                            languageSql,
                            new { Id = expenseReceiptDocumentDto.OperationDocument.DocumentLanguageId }
                        );

                        if (languageInfo != null)
                        {
                            expenseReceiptDocumentDto.OperationDocument.LanguageCode = languageInfo;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogAndThrow(nameof(ExpenseReceiptDocumentDal), $"An error occurred while querying the expense tables for AnnotationId: {annotationId}. Error: {ex.Message}", ex);
                }
                finally
                {
                    idbConnection.Close();
                }
            }

            return expenseReceiptDocumentDto;
        }

        private void ValidateSqlConnectionString(string? connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                var errorMsg = "Anomaly: The connection string 'APPX-Claim-ConnectionString' is null or empty.";
                _logger.LogAndThrow(nameof(ExpenseReceiptDocumentDal), errorMsg);
            }

            // Basic validation to check if it's a SQL connection string
            var requiredKeywords = new[] { "Server", "Initial Catalog" };
            var isValidSqlConnectionString = requiredKeywords.All(keyword => connectionString!.Contains(keyword, StringComparison.OrdinalIgnoreCase));

            if (!isValidSqlConnectionString)
            {
                _logger.LogAndThrow(nameof(ExpenseReceiptDocumentDal), "The connection string 'APPX-Claim-ConnectionString' does not appear to be a valid SQL connection string.");
            }
        }
    }
}
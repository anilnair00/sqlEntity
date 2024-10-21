using AirCanada.Appx.Claim.DataAccess.Expense.Dtos;
using Microsoft.Extensions.Logging;

namespace AirCanada.Appx.Claim.DataAccess.Expense.Dals
{
    public interface IExpenseReceiptDocumentDal
    {
        ExpenseReceiptDocumentDto? Fetch(Guid annotationId, ILogger<IExpenseReceiptDocumentDal> logger);
        ExpenseReceiptDocumentDto? QueryExpenseTables(Guid annotationId, ILogger<IExpenseReceiptDocumentDal> logger);
    }
}

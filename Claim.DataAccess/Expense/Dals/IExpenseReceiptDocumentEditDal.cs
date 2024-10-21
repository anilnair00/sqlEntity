using AirCanada.Appx.Claim.DataAccess.Expense.Dtos;

namespace AirCanada.Appx.Claim.DataAccess.Expense.Dals
{
    public interface IExpenseReceiptDocumentEditDal
    {
        public void Update(ExpenseReceiptDocumentEditDto dto);
        public ExpenseReceiptDocumentEditDto? Fetch(long ssetOperationDocumentsId);
    }
}

using AirCanada.Appx.Claim.DataAccess.Expense.Models;

namespace AirCanada.Appx.Claim.DataAccess.Expense.Dtos
{
    public class ExpenseReceiptDocumentDto
    {
        public ExpenseReceiptDocumentDto()
        {
            Operation = new SsetOperationEntity();
            OperationExpense = new SsetOperationExpenseEntity();
            OperationDocument = new SsetOperationDocumentEntity();
        }

        // SsetOperation related properties
        public SsetOperationEntity Operation { get; set; }

        // SsetOperationExpense related properties
        public SsetOperationExpenseEntity OperationExpense { get; set; }

        // SsetOperationDocument related properties
        public SsetOperationDocumentEntity OperationDocument { get; set; }
    }
}

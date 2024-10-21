using AirCanada.Appx.Claim.DataAccess.Expense.Dals;
using AirCanada.Appx.Claim.DataAccess.Expense.Dtos;
using AirCanada.Appx.Common.Extensions;
using Csla;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AirCanada.Appx.Claim.Expense
{
    [Serializable]
    public class ExpenseReceiptDocumentEdit : BusinessBase<ExpenseReceiptDocumentEdit>
    {
        public static readonly PropertyInfo<long?> IdProperty = RegisterProperty<long?>(c => c.Id);
        [Required]
        public long? Id
        {
            get { return GetProperty(IdProperty); }
            set { SetProperty(IdProperty, value); }
        }

        public static readonly PropertyInfo<decimal?> ExtractedAmountProperty = RegisterProperty<decimal?>(c => c.ExtractedAmount);
        public decimal? ExtractedAmount
        {
            get { return GetProperty(ExtractedAmountProperty); }
            set { SetProperty(ExtractedAmountProperty, value); }
        }

        public static readonly PropertyInfo<bool?> isValidAmountProperty = RegisterProperty<bool?>(c => c.IsValidAmount);
        [Required]
        public bool? IsValidAmount
        {
            get { return GetProperty(isValidAmountProperty); }
            set { SetProperty(isValidAmountProperty, value); }
        }

        public static readonly PropertyInfo<bool?> isValidDateProperty = RegisterProperty<bool?>(c => c.IsValidDate);
        [Required]
        public bool? IsValidDate
        {
            get { return GetProperty(isValidDateProperty); }
            set { SetProperty(isValidDateProperty, value); }
        }

        public static readonly PropertyInfo<DateTime?> ExtractedDateProperty = RegisterProperty<DateTime?>(c => c.ExtractedDate);
        public DateTime? ExtractedDate
        {
            get { return GetProperty(ExtractedDateProperty); }
            set { SetProperty(ExtractedDateProperty, value); }
        }

        [Update]
        private void Update([Inject] IExpenseReceiptDocumentEditDal dal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new ExpenseReceiptDocumentEditDto
                {
                    Id = this.Id,
                    IsValidAmount = this.IsValidAmount,
                    IsValidDate = this.IsValidDate,
                    ExtractedAmount = this.ExtractedAmount,
                    ExtractedDate = this.ExtractedDate
                };
                dal.Update(dto);
            }
        }

        [Fetch]
        private void Fetch([Inject] IExpenseReceiptDocumentEditDal dal, [Inject] ILogger<ExpenseReceiptDocumentEdit> logger, long id)
        {
            var expenseReceiptDocumentEditDto = dal.Fetch(id);

            if (expenseReceiptDocumentEditDto == null)
            {
                var errorMsg = $"ExpenseReceiptDocument with ID {id} was not found.";
                logger.LogAndThrow(nameof(ExpenseReceiptDocumentEdit), errorMsg);
            }

            this.Id = expenseReceiptDocumentEditDto!.Id;
            this.ExtractedAmount = expenseReceiptDocumentEditDto.ExtractedAmount;
            this.IsValidAmount = expenseReceiptDocumentEditDto.IsValidAmount;
            this.IsValidDate = expenseReceiptDocumentEditDto.IsValidDate;
            this.ExtractedDate = expenseReceiptDocumentEditDto.ExtractedDate;

            BusinessRules.CheckRules();
        }
    }
}
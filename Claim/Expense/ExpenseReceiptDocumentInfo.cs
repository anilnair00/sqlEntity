using AirCanada.Appx.Claim.DataAccess.Expense.Dals;
using AirCanada.Appx.Common.Enum;
using Csla;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace AirCanada.Appx.Claim.Expense
{
    [Serializable]
    public class ExpenseReceiptDocumentInfo : ReadOnlyBase<ExpenseReceiptDocumentInfo>
    {
        public static readonly PropertyInfo<long> SsetOperationDocumentIdProperty = RegisterProperty<long>(c => c.SsetOperationDocumentId);
        public long SsetOperationDocumentId
        {
            get { return GetProperty(SsetOperationDocumentIdProperty); }
        }

        public static readonly PropertyInfo<long> SsetOperationExpenseIdProperty = RegisterProperty<long>(c => c.SsetOperationExpenseId);
        public long SsetOperationExpenseId
        {
            get { return GetProperty(SsetOperationExpenseIdProperty); }
        }

        public static readonly PropertyInfo<long> SsetOperationIdProperty = RegisterProperty<long>(c => c.SsetOperationId);
        public long SsetOperationId
        {
            get { return GetProperty(SsetOperationIdProperty); }
        }

        public static readonly PropertyInfo<string> FileNameProperty = RegisterProperty<string>(c => c.FileName);
        public string FileName
        {
            get { return GetProperty(FileNameProperty); }
        }

        public static readonly PropertyInfo<int> LanguageIdProperty = RegisterProperty<int>(c => c.LanguageId);
        public int LanguageId
        {
            get { return GetProperty(LanguageIdProperty); }
        }

        public static readonly PropertyInfo<long> SizeProperty = RegisterProperty<long>(c => c.Size);
        public long Size
        {
            get { return GetProperty(SizeProperty); }
        }

        public static readonly PropertyInfo<string> StorageContainerProperty = RegisterProperty<string>(c => c.StorageContainer);
        public string StorageContainer
        {
            get { return GetProperty(StorageContainerProperty); }
        }

        public static readonly PropertyInfo<string> StorageIdentifierProperty = RegisterProperty<string>(c => c.StorageIdentifier);
        public string StorageIdentifier
        {
            get { return GetProperty(StorageIdentifierProperty); }
        }

        public static readonly PropertyInfo<string> StoragePathProperty = RegisterProperty<string>(c => c.StoragePath);
        public string StoragePath
        {
            get { return GetProperty(StoragePathProperty); }
        }

        public static readonly PropertyInfo<ExpenseTypeEnum> ExpenseTypeProperty = RegisterProperty<ExpenseTypeEnum>(c => c.ExpenseType);
        [Required]
        public ExpenseTypeEnum ExpenseType
        {
            get { return GetProperty(ExpenseTypeProperty); }
        }

        public static readonly PropertyInfo<decimal> TotalAmountProperty = RegisterProperty<decimal>(c => c.TotalAmount);
        [Required]
        public decimal TotalAmount
        {
            get { return GetProperty(TotalAmountProperty); }
        }

        public static readonly PropertyInfo<SmartDate> TransactionDateProperty = RegisterProperty<SmartDate>(c => c.TransactionDate);
        public SmartDate TransactionDate
        {
            get { return GetProperty(TransactionDateProperty); }
        }

        public static readonly PropertyInfo<SmartDate> CheckInDateProperty = RegisterProperty<SmartDate>(c => c.CheckInDate);
        public SmartDate CheckInDate
        {
            get { return GetProperty(CheckInDateProperty); }
        }

        public static readonly PropertyInfo<SmartDate> CheckOutDateProperty = RegisterProperty<SmartDate>(c => c.CheckOutDate);
        public SmartDate CheckOutDate
        {
            get { return GetProperty(CheckOutDateProperty); }
        }

        public static readonly PropertyInfo<int> CurrencyIdProperty = RegisterProperty<int>(c => c.CurrencyId);
        public int CurrencyId
        {
            get { return GetProperty(CurrencyIdProperty); }
        }

        public static readonly PropertyInfo<string> CurrencyCodeProperty = RegisterProperty<string>(c => c.CurrencyCode);
        public string CurrencyCode
        {
            get { return GetProperty(CurrencyCodeProperty); }
        }

        public static readonly PropertyInfo<string> CurrencySymbolProperty = RegisterProperty<string>(c => c.CurrencySymbol);
        public string CurrencySymbol
        {
            get { return GetProperty(CurrencySymbolProperty); }
        }

        public static readonly PropertyInfo<string> LanguageCodeProperty = RegisterProperty<string>(c => c.LanguageCode);
        public string LanguageCode
        {
            get { return GetProperty(LanguageCodeProperty); }
        }

        public static readonly PropertyInfo<Guid> DynamicExpenseWebRequestIDProperty = RegisterProperty<Guid>(c => c.DynamicExpenseWebRequestID);
        public Guid DynamicExpenseWebRequestID
        {
            get { return GetProperty(DynamicExpenseWebRequestIDProperty); }
        }

        public static readonly PropertyInfo<Guid> DynamicsAnnotationWebRequestIdProperty = RegisterProperty<Guid>(c => c.DynamicsAnnotationWebRequestId);
        public Guid DynamicsAnnotationWebRequestId
        {
            get { return GetProperty(DynamicsAnnotationWebRequestIdProperty); }
        }

        [Fetch]
        private void Fetch([Inject] IExpenseReceiptDocumentDal dal, Guid annotationId, [Inject] ILogger<IExpenseReceiptDocumentDal> logger)
        {
            try
            {
                var dto = dal.Fetch(annotationId, logger);

                if (dto is not null)
                {
                    LoadProperty(DynamicsAnnotationWebRequestIdProperty, annotationId);
                    LoadProperty(SsetOperationDocumentIdProperty, dto.OperationDocument.Id);
                    LoadProperty(SsetOperationExpenseIdProperty, dto.OperationExpense.Id);
                    LoadProperty(SsetOperationIdProperty, dto.Operation.Id);
                    LoadProperty(ExpenseTypeProperty, dto.OperationExpense.ExpenseTypeId);
                    LoadProperty(TotalAmountProperty, dto.OperationExpense.Amount);
                    LoadProperty(CheckInDateProperty, dto.OperationExpense.CheckInDate);
                    LoadProperty(CheckOutDateProperty, dto.OperationExpense.CheckOutDate);
                    LoadProperty(CurrencyIdProperty, dto.OperationExpense.CurrencyId);
                    LoadProperty(TransactionDateProperty, dto.OperationExpense.TransactionDate);
                    LoadProperty(SsetOperationIdProperty, dto.Operation.Id);
                    LoadProperty(LanguageIdProperty, dto.OperationDocument.DocumentLanguageId);
                    LoadProperty(SizeProperty, dto.OperationDocument.DocumentSize);
                    LoadProperty(DynamicExpenseWebRequestIDProperty, dto.OperationExpense.DynamicExpenseWebRequestID);

                    if (dto.OperationExpense.CurrencyCode != null)
                    {
                        LoadProperty(CurrencyCodeProperty, dto.OperationExpense.CurrencyCode);
                    }
                    if (dto.OperationExpense.CurrencySymbol != null)
                    {
                        LoadProperty(CurrencySymbolProperty, dto.OperationExpense.CurrencySymbol);
                    }
                    if (dto.OperationDocument.FileName != null)
                    {
                        LoadProperty(FileNameProperty, dto.OperationDocument.FileName);
                    }
                    if (dto.OperationDocument.LanguageCode != null)
                    {
                        LoadProperty(LanguageCodeProperty, dto.OperationDocument.LanguageCode);
                    }
                    if (dto.OperationDocument.StorageContainer != null)
                    {
                        LoadProperty(StorageContainerProperty, dto.OperationDocument.StorageContainer);
                    }
                    if (dto.OperationDocument.StorageIdentifier != null)
                    {
                        LoadProperty(StorageIdentifierProperty, dto.OperationDocument.StorageIdentifier);
                    }
                    if (dto.OperationDocument.StoragePath != null)
                    {
                        LoadProperty(StoragePathProperty, dto.OperationDocument.StoragePath);
                    }
                }
                else
                {
                    throw new InvalidOperationException($"{nameof(ExpenseReceiptDocumentInfo)}: ExpenseReceiptDocument record not found for AnnotationId: {annotationId}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error@{nameof(ExpenseReceiptDocumentInfo)}: Inner Exception: {ex.Message}");
                throw new Exception($"Exception@{nameof(ExpenseReceiptDocumentInfo)}: Fault on fetching data for AnnotationId: {annotationId}. Exception: {ex.Message}", ex);
            }
        }
    }
}

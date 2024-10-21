using AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable;
using AzAiIntegrationEntity = AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable.AzAiIntegration;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.Test.ReceiptReader.Dals
{
    public static class DatabaseSeeder
    {
        public static void SeedDatabase(AppxDbContext dbContext)
        {
            dbContext.AzAiIntegrations.Add(new AzAiIntegrationEntity
            {
                Id = 98L,
                CreatedDateTime = DateTimeOffset.UtcNow,
                RequestMsg = "{\"MessageContext\":{\"RequestId\":98,\"CreatedDateTime\":\"2024-09-30T15:12:37.8426813+00:00\",\"Environment\":\"DEV\",\"Version\":\"\",\"SsetOperationId\":2438,\"SsetExpenseId\":186,\"SsetDocumentId\":95,\"DynamicExpenseWebRequestID\":\"f92ede07-7d23-ef11-840a-7c1e520a0f04\",\"DynamicsAnnotationWebRequestId\":\"fa2ede07-7d23-ef11-840a-7c1e520a0f04\"},\"Currency\":{\"Code\":\"CAD\",\"Symbol\":\"$\"},\"Document\":{\"FileName\":\"fake_receipt.png\",\"LanguageCode\":\"en-CA\",\"Size\":4259,\"StorageIdentifier\":\"stacpcatexpensedev02\",\"StorageContainer\":\"92b465e8-7d23-ef11-840a-7c1e520a0f04\",\"StoragePath\":\"https://stacaccxappxdev01.blob.core.windows.net/expense-receipts/2024/09/92b465e8-7d23-ef11-840a-7c1e520a0f04/meal/fa2ede07-7d23-ef11-840a-7c1e520a0f04.jpg\"},\"ExpenseTypeCode\":\"ML\",\"TotalAmount\":{\"InputContent\":\"250.00\",\"CalibrationType\":\"absolute\",\"CalibrationValue\":\"0.95\"},\"TransactionDate\":{\"InputContent\":\"0001-01-01\"},\"Stage\":\"Queuing\",\"State\":\"New\"}",
                SsetDocumentId = 95L,
                MessageMetaData = "{\"DynamicExpenseWebRequestID\":\"f92ede07-7d23-ef11-840a-7c1e520a0f04\",\"DynamicsAnnotationWebRequestId\":\"fa2ede07-7d23-ef11-840a-7c1e520a0f04\",\"SsetOperationId\":2438,\"SsetExpenseId\":186,\"SsetDocumentId\":95,\"Version\":\"\"}",
                Stage = "Processing",
                State = "Queued"
            });

            dbContext.SaveChanges();
        }
    }
}
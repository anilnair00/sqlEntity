using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dtos;
using AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable;
using AirCanada.Appx.Common.Extensions;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals
{
    public class ReceiptReaderRequestEditDal : IReceiptReaderRequestEditDal
    {
        private readonly AppxDbContext _dbContext;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly ILogger<ReceiptReaderRequestEditDal> _logger;

        public ReceiptReaderRequestEditDal(AppxDbContext dbContext, ILogger<ReceiptReaderRequestEditDal> logger)
        {
            _dbContext = dbContext;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };
            _logger = logger;
        }

        public long Insert(ReceiptReaderRequestDto dto)
        {
            if (dto is null)
            {
                string errorMsg = "The provided DTO is null.";
                var ex = new ArgumentNullException(nameof(dto), errorMsg);
                _logger.LogAndThrow(nameof(ReceiptReaderRequestEditDal), errorMsg, ex);
            }

            var azAiIntegrationEntity = ConvertDtoToEntity(dto!);
            _dbContext.AzAiIntegrations.Add(azAiIntegrationEntity);
            _dbContext.SaveChanges();

            var generatedId = azAiIntegrationEntity.Id;
            dto!.MessageContext.RequestId = generatedId;
            azAiIntegrationEntity.RequestMsg = JsonSerializer.Serialize(dto, _jsonSerializerOptions);

            _dbContext.SaveChanges();
            return generatedId;
            // TODO: Surround the code by a try/catch and log the exception.
        }

        private static SQLTable.AzAiIntegration ConvertDtoToEntity(ReceiptReaderRequestDto dto)
        {
            var metaData = new
            {
                DynamicExpenseWebRequestID = dto.MessageContext.DynamicExpenseWebRequestID,
                DynamicsAnnotationWebRequestId = dto.MessageContext.DynamicsAnnotationWebRequestId,
                SsetOperationId = dto.MessageContext.SsetOperationId,
                SsetExpenseId = dto.MessageContext.SsetExpenseId,
                SsetDocumentId = dto.MessageContext.SsetDocumentId,
                Version = dto.MessageContext.Version
            };

            return new SQLTable.AzAiIntegration
            {
                Id = dto.MessageContext.RequestId,
                CreatedDateTime = DateTimeOffset.UtcNow,
                SsetDocumentId = dto.MessageContext.SsetDocumentId,
                MessageMetaData = JsonSerializer.Serialize(metaData),
                ResponseMsg = string.Empty,
                Stage = dto.Stage?.ToString(),
                State = dto.State?.ToString()
            };
        }
    }
}

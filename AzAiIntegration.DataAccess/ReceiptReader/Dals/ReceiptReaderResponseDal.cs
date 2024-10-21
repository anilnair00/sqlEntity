using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dtos;
using AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable;
using AirCanada.Appx.Common.Extensions;
using Microsoft.Extensions.Logging;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals
{
    public class ReceiptReaderResponseDal : IReceiptReaderResponseDal
    {
        private readonly AppxDbContext _dbContext;
        private readonly ILogger<ReceiptReaderResponseDal> _logger;

        public ReceiptReaderResponseDal(AppxDbContext dbContext, ILogger<ReceiptReaderResponseDal> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public void Update(ReceiptReaderResponseDto dto)
        {
            if (dto == null)
            {
                string errorMsg = "The provided DTO is null.";
                var ex = new ArgumentNullException(nameof(dto), errorMsg);
                _logger.LogAndThrow(nameof(ReceiptReaderResponseDal), errorMsg, ex);
            }

            if (dto!.MessageContext == null)
            {
                string errorMsg = "The provided DTO MessageContext is null.";
                var ex = new ArgumentNullException(nameof(dto.MessageContext), errorMsg);
                _logger.LogAndThrow(nameof(ReceiptReaderResponseDal), errorMsg, ex);
            }

            if (dto.MessageContext!.CorrelationId == null)
            {
                string errorMsg = "The provided DTO MessageContext CorrelationId is null.";
                var ex = new ArgumentNullException(nameof(dto.MessageContext.CorrelationId), errorMsg);
                _logger.LogAndThrow(nameof(ReceiptReaderResponseDal), errorMsg, ex);
            }

            var azAiIntegration = _dbContext.AzAiIntegrations.Find(dto.Id);

            if (azAiIntegration == null)
            {
                string errorMsg = "AzAiIntegration record does not exist.";
                _logger.LogAndThrow(nameof(ReceiptReaderResponseDal), errorMsg);
            }

            azAiIntegration!.ResponseMsg = dto.AiResponseMessage;
            azAiIntegration.UpdatedDateTime = DateTimeOffset.Now;
            azAiIntegration.ResponseId = dto.MessageContext!.Responseid;

            _dbContext.SaveChanges();
        }

        public ReceiptReaderResponseDto? Fetch(long requestId)
        {
            ReceiptReaderResponseDto? dto = null;

            if (requestId <= 0L)
            {
                string errorMsg = $"Invalid request ID: {requestId}. Request ID must be greater than zero.";
                var ex = new ArgumentNullException(nameof(requestId), errorMsg);
                _logger.LogAndThrow(nameof(ReceiptReaderResponseDal), errorMsg, ex);
            }

            try
            {
                var azAiIntegrationEntity = _dbContext.AzAiIntegrations.Find(requestId);

                if (azAiIntegrationEntity == null)
                {
                    return null;
                }

                dto = new ReceiptReaderResponseDto
                {
                    Id = azAiIntegrationEntity.Id,
                    AiResponseMessage = azAiIntegrationEntity.ResponseMsg,
                    SsetDocumentId = azAiIntegrationEntity.SsetDocumentId
                };

                dto.MessageContext!.Responseid = azAiIntegrationEntity.ResponseId;
                dto.MessageContext.CorrelationId = azAiIntegrationEntity.Id;
                dto.MessageContext.CreatedDateTime = azAiIntegrationEntity.CreatedDateTime;

                return dto;
            }
            catch (Exception ex)
            {
                string errorMsg = $"Failed to fetch the record for Request ID: {requestId}";
                _logger.LogAndThrow(nameof(ReceiptReaderResponseDal), errorMsg, ex);
            }

            return dto;
        }
    }
}
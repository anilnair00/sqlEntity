using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dtos;
using AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable;
using AirCanada.Appx.Common.Extensions;
using Microsoft.Extensions.Logging;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals
{
    public class ReceiptReaderRequestStateEditDal : IReceiptReaderRequestStateEditDal
    {
        private readonly AppxDbContext _appxDbContext;
        private readonly ILogger<ReceiptReaderRequestStateEditDal> _logger;

        public ReceiptReaderRequestStateEditDal(AppxDbContext appxDbContext, ILogger<ReceiptReaderRequestStateEditDal> logger)
        {
            _appxDbContext = appxDbContext;
            _logger = logger;
        }

        public ReceiptReaderRequestStateDto Fetch(long id)
        {
            var entity = _appxDbContext.AzAiIntegrations.Find(id);
            if (entity == null)
            {
                _logger.LogAndThrow<ReceiptReaderRequestStateEditDal>("ReceiptReaderRequestStateEditDal", $"Record with Id {id} not found");
            }

            return new ReceiptReaderRequestStateDto
            {
                Id = entity!.Id,
                Stage = entity.Stage,
                State = entity.State
            };
        }

        public void Update(ReceiptReaderRequestStateDto dto)
        {
            var entity = _appxDbContext.AzAiIntegrations.Find(dto.Id);

            if (entity == null)
            {
                _logger.LogAndThrow<ReceiptReaderRequestStateEditDal>("ReceiptReaderRequestStateEditDal", $"Record with Id {dto.Id} not found");
            }

            // Log the transition information
            var beforeStage = entity!.Stage;
            var beforeState = entity.State;
            var afterStage = dto.Stage;
            var afterState = dto.State;
            _logger.LogInformation("{HandlerName} => Transitioning ID {Id}: Stage from '{BeforeStage}' to '{AfterStage}', State from '{BeforeState}' to '{AfterState}'",
                nameof(ReceiptReaderRequestStateEditDal), dto.Id, beforeStage, afterStage, beforeState, afterState);

            entity.Stage = dto.Stage;
            entity.State = dto.State;

            _appxDbContext.SaveChanges();
        }
    }
}
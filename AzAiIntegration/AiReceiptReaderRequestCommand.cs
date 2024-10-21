using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals;
using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Messages;
using AirCanada.Appx.Common.Enum;
using AutoMapper;
using Csla;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace AirCanada.Appx.AzAiIntegration
{
    [Serializable]
    public class AiReceiptReaderRequestCommand : CommandBase<AiReceiptReaderRequestCommand>
    {
        public static readonly PropertyInfo<ReceiptReaderRequestEdit> ReceiptReaderRequestEditProperty = RegisterProperty<ReceiptReaderRequestEdit>(c => c.ReceiptReaderRequestEdit);
        [Required]
        public ReceiptReaderRequestEdit ReceiptReaderRequestEdit
        {
            get { return ReadProperty(ReceiptReaderRequestEditProperty); }
            set { LoadProperty(ReceiptReaderRequestEditProperty, value); }
        }

        [Create]
        private void Create(ReceiptReaderRequestEdit requestEdit)
        {
            ReceiptReaderRequestEdit = requestEdit;
        }

        [Execute]
        private async Task Execute([Inject] IAiReceiptReaderRequestDal dal, [Inject] IMapper mapper, [Inject] ILogger<AiReceiptReaderRequestCommand> logger)
        {
            if (ReceiptReaderRequestEdit.IsValid == true)
            {
                logger.LogInformation("{TypeName} => Mapping AzAiIntegration record - {PropertyName}: {Id}", nameof(AiReceiptReaderRequestCommand), nameof(ReceiptReaderRequestEdit.Id), this.ReceiptReaderRequestEdit.Id);

                var aiReceiptReaderRequestMsg = mapper.Map<AiReceiptReaderRequestMsg>(this.ReceiptReaderRequestEdit);

                await dal.Insert(aiReceiptReaderRequestMsg);
            }
            else
            {
                ReceiptReaderRequestEdit.State = StateEnum.Failed;
                throw new Csla.Rules.ValidationException();
            }
        }
    }
}
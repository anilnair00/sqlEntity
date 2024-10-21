using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals;
using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dtos;
using Csla;
using System.Diagnostics.CodeAnalysis;

namespace AirCanada.Appx.AzAiIntegration
{
    [Serializable]
    public class ReceiptReaderRequestStateEdit : BusinessBase<ReceiptReaderRequestStateEdit>
    {
        public static readonly PropertyInfo<long> IdProperty = RegisterProperty<long>(c => c.Id);
        public long Id
        {
            get => GetProperty(IdProperty);
            set => SetProperty(IdProperty, value);
        }

        public static readonly PropertyInfo<Common.Enum.StageEnum?> StageProperty = RegisterProperty<Common.Enum.StageEnum?>(c => c.Stage);
        public Common.Enum.StageEnum? Stage
        {
            get => GetProperty(StageProperty);
            set => SetProperty(StageProperty, value);
        }

        public static readonly PropertyInfo<Common.Enum.StateEnum?> StateProperty = RegisterProperty<Common.Enum.StateEnum?>(c => c.State);
        public Common.Enum.StateEnum? State
        {
            get => GetProperty(StateProperty);
            set => SetProperty(StateProperty, value);
        }

        [Fetch]
        [SuppressMessage("Style", "IDE0051:Remove unused private members", Justification = "Used by CSLA data portal")]
        private void Fetch(long id, [Inject] IReceiptReaderRequestStateEditDal dal)
        {
            var dto = dal.Fetch(id);
            LoadProperty(IdProperty, dto.Id);
            LoadProperty(StageProperty, Enum.TryParse<Common.Enum.StageEnum>(dto.Stage, out var stage) ? stage : (Common.Enum.StageEnum?)null);
            LoadProperty(StateProperty, Enum.TryParse<Common.Enum.StateEnum>(dto.State, out var state) ? state : (Common.Enum.StateEnum?)null);
        }

        [Update]
        [SuppressMessage("Style", "IDE0051:Remove unused private members", Justification = "Used by CSLA data portal")]
        private void DataPortal_Update([Inject] IReceiptReaderRequestStateEditDal dal)
        {
            var dto = new ReceiptReaderRequestStateDto
            {
                Id = Id,
                Stage = Stage?.ToString(),
                State = State?.ToString()
            };

            dal.Update(dto);
        }
    }
}
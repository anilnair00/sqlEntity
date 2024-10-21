using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals;
using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Messages;
using AirCanada.Appx.Common.Wrappers;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.Test.ReceiptReader
{
    public class AiReceiptReaderRequestDalTests
    {
        private readonly AiReceiptReaderRequestDalTestsMock _mock;
        private readonly AiReceiptReaderRequestDal _aiReceiptReaderRequestDal;
        private readonly Mock<IServiceBusClientWrapper> _serviceBusClientWrapperMock = new Mock<IServiceBusClientWrapper>();
        private readonly Mock<ILogger<AiReceiptReaderRequestDal>> _loggerMock = new Mock<ILogger<AiReceiptReaderRequestDal>>();

        public AiReceiptReaderRequestDalTests()
        {
            _aiReceiptReaderRequestDal = new AiReceiptReaderRequestDal(_serviceBusClientWrapperMock.Object, new ConfigurationBuilder().Build(), _loggerMock.Object);
            _mock = new AiReceiptReaderRequestDalTestsMock();
        }

        [Fact]
        public async Task InsertNullDtoToBusFailsValidation()
        {
            AiReceiptReaderRequestMsg? msg = null;

            var exception = await Record.ExceptionAsync(() => _aiReceiptReaderRequestDal.Insert(msg!));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<InvalidOperationException>();
            exception.InnerException.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
            exception.InnerException.As<ArgumentNullException>().ParamName.Should().Be("aiReceiptReaderRequestMsg");
        }
    }
}

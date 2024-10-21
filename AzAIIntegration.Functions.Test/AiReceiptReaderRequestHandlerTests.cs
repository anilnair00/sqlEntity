using AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable;
using AirCanada.Appx.Claim.Test;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace AirCanada.Appx.AzAiIntegration.Functions.Test
{
    public class AiReceiptReaderRequestHandlerTests : IClassFixture<CslaTestFixture>
    {
        private readonly Mock<AppxDbContext> _appxDbContextMock;
        private readonly Mock<ILogger<AiReceiptReaderRequestHandler>> _loggerMock;

        public AiReceiptReaderRequestHandlerTests()
        {
            _appxDbContextMock = new Mock<AppxDbContext>();
            _loggerMock = new Mock<ILogger<AiReceiptReaderRequestHandler>>();
        }

        [Fact]
        public void ExtractAnnotationId_ShouldSucceed()
        {
            //Arrange
            var fileName = "bla-bla-bla-D643D852-3C27-EF11-840B-000D3A99B674-bla";
            var expectedGuid = Guid.Parse("D643D852-3C27-EF11-840B-000D3A99B674");
            var requestHandler = new AiReceiptReaderRequestHandler(_loggerMock.Object, null, new ConfigurationBuilder().Build());

            //Act
            var annotationId = requestHandler.ExtractAnnotationId(fileName);

            //Assert
            annotationId.Should().Be(expectedGuid);
        }

        [Fact]
        public void ExtractAnnotationId_ShouldFail()
        {
            //Arrange
            var fileName = "invalid-document-name";
            var requestHandler = new AiReceiptReaderRequestHandler(_loggerMock.Object, null, new ConfigurationBuilder().Build());

            //Act
            var exception = Record.Exception(() => requestHandler.ExtractAnnotationId(fileName));

            //Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<InvalidOperationException>();
        }
    }
}
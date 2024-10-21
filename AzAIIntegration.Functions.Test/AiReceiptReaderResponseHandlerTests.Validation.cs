using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Messages;
using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Messages.Details;
using AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable;
using Azure.Messaging.ServiceBus;
using Csla;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;
using System.Text.Json;

namespace AirCanada.Appx.AzAiIntegration.Functions.Test
{
    public partial class AiReceiptReaderResponseHandlerTests : IClassFixture<FunctionTestFixture>
    {
        [Fact]
        public async Task ShouldThrowArgumentNullException_WhenMessageIsNull()
        {
            // Arrange
            var logger = _serviceProvider.GetRequiredService<ILogger<AiReceiptReaderResponseHandler>>();
            var dbContext = new Mock<AppxDbContext>(new DbContextOptions<AppxDbContext>()).Object;
            var portalFactory = new Mock<IDataPortalFactory>().Object;

            var handler = new AiReceiptReaderResponseHandler(_configurationMock.Object, portalFactory, logger);

            ServiceBusReceivedMessage? message = null;
            var messageActionsMock = new Mock<ServiceBusMessageActions>();

            // Act
            async Task act() => await handler.Run(message!, messageActionsMock.Object);

            // Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
            exception.Message.Should().Contain("Service Bus Message is null.");

            // Check inner exception details
            exception.InnerException.Should().NotBeNull();
            exception.InnerException.Should().BeOfType<ArgumentNullException>();
            exception.InnerException!.Message.Should().Contain("Value cannot be null. (Parameter 'message')");
        }

        [Fact]
        public async Task ShouldThrowArgumentNullException_WhenMessageIdIsNullOrEmpty()
        {
            // Arrange
            var logger = _serviceProvider.GetRequiredService<ILogger<AiReceiptReaderResponseHandler>>();
            var dbContext = new Mock<AppxDbContext>(new DbContextOptions<AppxDbContext>()).Object;
            var portalFactory = new Mock<IDataPortalFactory>().Object;

            var handler = new AiReceiptReaderResponseHandler(_configurationMock.Object, portalFactory, logger);

            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(
                            body: new BinaryData(Encoding.UTF8.GetBytes("{}")),
                            messageId: null, // MessageId is null
                            contentType: "application/json");

            var messageActionsMock = new Mock<ServiceBusMessageActions>();

            // Act
            async Task act() => await handler.Run(message, messageActionsMock.Object);

            // Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
            exception.Message.Should().Contain("Message ID is null or empty.");

            // Check inner exception details
            exception.InnerException.Should().NotBeNull();
            exception.InnerException.Should().BeOfType<ArgumentNullException>();
            exception.InnerException!.Message.Should().Contain("Value cannot be null. (Parameter 'MessageId')");
        }

        [Fact]
        public async Task ShouldThrowArgumentNullException_WhenMessageContentTypeIsNullOrEmpty()
        {
            // Arrange
            var logger = _serviceProvider.GetRequiredService<ILogger<AiReceiptReaderResponseHandler>>();
            var dbContext = new Mock<AppxDbContext>(new DbContextOptions<AppxDbContext>()).Object;
            var portalFactory = new Mock<IDataPortalFactory>().Object;

            var handler = new AiReceiptReaderResponseHandler(_configurationMock.Object, portalFactory, logger);

            var messageBody = JsonSerializer.Serialize(new AiReceiptReaderResponseMsg
            {
                MessageContext = new ResponseMessageContextMsgDetail
                {
                    CorrelationId = null // CorrelationId is null
                }
            });
            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(
                            body: new BinaryData(Encoding.UTF8.GetBytes(messageBody)),
                            messageId: "test-message-id",
                            contentType: null); // ContentType is null

            var messageActionsMock = new Mock<ServiceBusMessageActions>();

            // Act
            async Task act() => await handler.Run(message, messageActionsMock.Object);

            // Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
            exception.Message.Should().Contain("Exception@AiReceiptReaderResponseHandler => Message content type is null or empty for Message ID: test-message-id.");

            // Check inner exception details
            exception.InnerException.Should().NotBeNull();
            exception.InnerException.Should().BeOfType<ArgumentNullException>();
            exception.InnerException!.Message.Should().Contain("Value cannot be null. (Parameter 'ContentType')");
        }

        [Fact]
        public async Task ShouldThrowArgumentException_WhenMessageContentTypeIsNotJson()
        {
            // Arrange
            var logger = _serviceProvider.GetRequiredService<ILogger<AiReceiptReaderResponseHandler>>();
            var dbContext = new Mock<AppxDbContext>(new DbContextOptions<AppxDbContext>()).Object;
            var portalFactory = new Mock<IDataPortalFactory>().Object;
            var handler = new AiReceiptReaderResponseHandler(_configurationMock.Object, portalFactory, logger);

            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(
                messageId: "test-message-id",
                contentType: "text/plain", // Invalid content type
                body: BinaryData.FromString("{}")
            );
            var messageActionsMock = new Mock<ServiceBusMessageActions>();

            // Act
            async Task act() => await handler.Run(message, messageActionsMock.Object);

            // Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
            exception.Message.Should().Contain("Message content type is not 'application/json'");

            // Check inner exception details
            exception.InnerException.Should().NotBeNull();
            exception.InnerException.Should().BeOfType<ArgumentException>();
            exception.InnerException!.Message.Should().Contain("ContentType");
        }

        [Fact]
        public async Task ShouldThrowArgumentNullException_WhenMessageBodyIsNull()
        {
            // Arrange
            var logger = _serviceProvider.GetRequiredService<ILogger<AiReceiptReaderResponseHandler>>();
            var dbContext = new Mock<AppxDbContext>(new DbContextOptions<AppxDbContext>()).Object;
            var portalFactory = new Mock<IDataPortalFactory>().Object;

            var handler = new AiReceiptReaderResponseHandler(_configurationMock.Object, portalFactory, logger);

            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(body: null, messageId: "test-message-id", contentType: "application/json");
            var messageActionsMock = new Mock<ServiceBusMessageActions>();

            // Act
            async Task act() => await handler.Run(message, messageActionsMock.Object);

            // Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
            exception.Message.Should().Contain("Message body is null for Message ID: test-message-id.");

            // Check inner exception details
            exception.InnerException.Should().NotBeNull();
            exception.InnerException.Should().BeOfType<ArgumentNullException>();
            exception.InnerException!.Message.Should().Contain("Value cannot be null. (Parameter 'Body')");
        }

        [Fact]
        public async Task ShouldThrowArgumentNullException_WhenMessageContextIsNull()
        {
            // Arrange
            var logger = _serviceProvider.GetRequiredService<ILogger<AiReceiptReaderResponseHandler>>();
            var dbContext = new Mock<AppxDbContext>(new DbContextOptions<AppxDbContext>()).Object;
            var portalFactory = new Mock<IDataPortalFactory>().Object;

            var handler = new AiReceiptReaderResponseHandler(_configurationMock.Object, portalFactory, logger);

            var aiReceiptReaderResponseMsg = new AiReceiptReaderResponseMsg
            {
                MessageContext = null
            };
            var messageBody = JsonSerializer.Serialize(aiReceiptReaderResponseMsg);
            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(
                            body: new BinaryData(Encoding.UTF8.GetBytes(messageBody)),
                            messageId: "test-message-id",
                            contentType: "application/json");

            var messageActionsMock = new Mock<ServiceBusMessageActions>();

            // Act
            async Task act() => await handler.Run(message, messageActionsMock.Object);

            // Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
            exception.Message.Should().Contain("Exception@AiReceiptReaderResponseHandler => Message context is null for Message ID: {message.MessageId}.");

            // Check inner exception details
            exception.InnerException.Should().NotBeNull();
            exception.InnerException.Should().BeOfType<ArgumentNullException>();
            exception.InnerException!.Message.Should().Contain("Value cannot be null. (Parameter 'MessageContext')");
        }

        [Fact]
        public async Task ShouldThrowArgumentNullException_WhenCorrelationIdIsNull()
        {
            // Arrange
            var logger = _serviceProvider.GetRequiredService<ILogger<AiReceiptReaderResponseHandler>>();
            var dbContext = new Mock<AppxDbContext>(new DbContextOptions<AppxDbContext>()).Object;
            var portalFactory = new Mock<IDataPortalFactory>().Object;

            var handler = new AiReceiptReaderResponseHandler(_configurationMock.Object, portalFactory, logger);

            var messageBody = JsonSerializer.Serialize(new AiReceiptReaderResponseMsg
            {
                MessageContext = new ResponseMessageContextMsgDetail
                {
                    CorrelationId = null // CorrelationId is null
                }
            });
            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(
                            body: new BinaryData(Encoding.UTF8.GetBytes(messageBody)),
                            messageId: "test-message-id",
                            contentType: "application/json");

            var messageActionsMock = new Mock<ServiceBusMessageActions>();

            // Act
            async Task act() => await handler.Run(message, messageActionsMock.Object);

            // Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
            exception.Message.Should().Contain("Exception@AiReceiptReaderResponseHandler => Missing correlation id (i.e. Request Id) for Message ID: {message.MessageId}.");

            // Check inner exception details
            exception.InnerException.Should().NotBeNull();
            exception.InnerException.Should().BeOfType<ArgumentNullException>();
            exception.InnerException!.Message.Should().Contain("Value cannot be null. (Parameter 'CorrelationId')");
        }

        [Fact]
        public async Task ShouldDeadLetterMessage_WhenDeserializationFails()
        {
            // Arrange
            var logger = _serviceProvider.GetRequiredService<ILogger<AiReceiptReaderResponseHandler>>();
            var dbContext = new Mock<AppxDbContext>(new DbContextOptions<AppxDbContext>()).Object;
            var portalFactory = new Mock<IDataPortalFactory>().Object;

            var handler = new AiReceiptReaderResponseHandler(_configurationMock.Object, portalFactory, logger);

            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(
                            body: new BinaryData(Encoding.UTF8.GetBytes("Invalid JSON")),
                            messageId: "test-message-id",
                            contentType: "application/json");

            var messageActionsMock = new Mock<ServiceBusMessageActions>();

            // Act
            async Task act() => await handler.Run(message, messageActionsMock.Object);

            // Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
            exception.Message.Should().Contain("Failed to deserialize the message body for Message ID: test-message-id.");

            // Check inner exception details
            exception.InnerException.Should().NotBeNull();
            exception.InnerException.Should().BeOfType<JsonException>();
            exception.InnerException!.Message.Should().Contain("'I' is an invalid start of a value. Path: $ | LineNumber: 0 | BytePositionInLine: 0.");
        }
    }
}
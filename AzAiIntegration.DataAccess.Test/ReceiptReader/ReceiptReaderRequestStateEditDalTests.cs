using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals;
using AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using AzAiIntegrationEntity = AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable.AzAiIntegration;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.Test.ReceiptReader
{
    public class ReceiptReaderRequestStateEditDalTests
    {
        [Fact]
        public void Fetch_ShouldReturnCorrectDto_WhenIdExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppxDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Fetch")
                .Options;

            using (var context = new AppxDbContext(options))
            {
                context.AzAiIntegrations.Add(new AzAiIntegrationEntity { Id = 95, Stage = "Processing", State = "Queued" });
                context.SaveChanges();
            }

            var mockLogger = new Mock<ILogger<ReceiptReaderRequestStateEditDal>>();

            using (var context = new AppxDbContext(options))
            {
                var dal = new ReceiptReaderRequestStateEditDal(context, mockLogger.Object);

                // Act
                var result = dal.Fetch(95);

                // Assert
                result.Should().NotBeNull();
                result.Id.Should().Be(95);
                result.Stage.Should().Be("Processing");
                result.State.Should().Be("Queued");
            }
        }

        [Fact]
        public void Update_ShouldSaveCorrectly_WhenStateTransitions()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppxDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Update")
                .Options;

            using (var context = new AppxDbContext(options))
            {
                context.AzAiIntegrations.Add(new AzAiIntegrationEntity { Id = 95, Stage = "Processing", State = "Queued" });
                context.SaveChanges();
            }

            var mockLogger = new Mock<ILogger<ReceiptReaderRequestStateEditDal>>();

            using (var context = new AppxDbContext(options))
            {
                var dal = new ReceiptReaderRequestStateEditDal(context, mockLogger.Object);

                // Act
                var fetchedDto = dal.Fetch(95);
                fetchedDto.Stage = "Acknowledgement";
                fetchedDto.State = "Processed";
                dal.Update(fetchedDto);

                var resultDto = dal.Fetch(95);

                // Assert
                resultDto.Should().NotBeNull();
                resultDto.Id.Should().Be(95);
                resultDto.Stage.Should().Be("Acknowledgement");
                resultDto.State.Should().Be("Processed");
            }
        }
    }
}
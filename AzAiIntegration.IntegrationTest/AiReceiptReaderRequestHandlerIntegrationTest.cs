using Azure.Storage.Blobs;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace AirCanada.Appx.AzAiIntegration.IntegrationTest
{
    public class AiReceiptReaderRequestHandlerIntegrationTest : IClassFixture<AzAiIntegrationTestFixture>
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;
        private readonly string? connectionString;

        public AiReceiptReaderRequestHandlerIntegrationTest(AzAiIntegrationTestFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
            _config = fixture.Configuration;

            connectionString = _config["APPX-BlobStorage-ConnectionString"];
        }

        public async Task UploadFileToBlobStorage_ShouldSucceed()
        {
            // Arrange
            var stream = new MemoryStream();
            var containerName = "expensereceipts";
            var blobName = Guid.NewGuid().ToString("N") + ".txt";
            var content = "Test content";

            using (var writer = new StreamWriter(stream))
            {
                writer.Write(content);
                writer.Flush();
                stream.Position = 0; // Set the position of the stream to the beginning

                // Act
                var serviceClient = new BlobServiceClient(connectionString);
                var containerClient = serviceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(blobName);
                await blobClient.UploadAsync(stream, true);

                // Assert
                (await blobClient.ExistsAsync()).Value.Should().BeTrue();

                // Cleanup the file
                // await blobClient.DeleteIfExistsAsync();

                true.Should().BeTrue("because this test should not run as it should not have a [Fact] attribute.");
            }
        }

        public void TriggerAzureFunction_ShouldSucceed()
        {
            // Arrange
            // TODO: Add some test receipt data to verify that it is correctly processed

            // Act
            // TODO: Call the Azure function with the test receipt data

            // Assert
            true.Should().BeTrue();
        }
    }
}
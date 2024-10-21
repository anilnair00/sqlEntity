using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using Xunit.Abstractions;

namespace AirCanada.Appx.AzAiIntegration.IntegrationTest
{
    public class APIM_Blob_IntegrationTest : IClassFixture<AzAiIntegrationTestFixture>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;
        private readonly ITestOutputHelper _output;

        public APIM_Blob_IntegrationTest(AzAiIntegrationTestFixture fixture, ITestOutputHelper output)
        {
            _config = fixture.Configuration;
            _serviceProvider = fixture.ServiceProvider;
            _output = output;
        }

        public async Task GETStorageBlobItem()
        {
            bool success = false;
            var container = "christopher";
            var item = "test.jpg";
            var SAS = _config["Values:SAS"];

            using (HttpClient client = new HttpClient())
            {
                var requestUri = new Uri($"https://azapi-dev.aircanada.com/cx-appx-claim-upload/{container}/{item}?{SAS}");

                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _config["Values:SubscriptionKey"]);

                try
                {
                    HttpResponseMessage response = await client.GetAsync(requestUri);
                    response.EnsureSuccessStatusCode();
                    success = true;

                    Assert.True(success);
                }
                catch (HttpRequestException e)
                {
                    _output.WriteLine($"Request error: {e.Message}");
                }
                catch (Exception e)
                {
                    _output.WriteLine($"Unexpected error: {e.Message}");
                }
            }
        }

        public async Task PUTStorageBlobItem()
        {
            bool success;
            var container = "christopher";
            var SAS = _config["Values:SAS"];

            // Ensure the file stream is properly disposed of
            using (Stream fileStream = new FileStream("C:\\Users\\AC263671\\Documents\\Dev\\repos\\AirCanada.Appx.Integration\\AzAiIntegration.IntegrationTest\\APIM\\receiptImages\\Walmart.JPG", FileMode.Open, FileAccess.Read))
            {
                using (HttpClient client = new HttpClient())
                {
                    // Ensure the file name is correctly formatted
                    //var item = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                    var item = "fromRepo.jpg";
                    var requestUri = new Uri($"https://azapi-dev.aircanada.com/cx-appx-claim-upload/{container}/{item}?{SAS}");
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _config["Values:SubscriptionKey"]);

                    using (var content = new StreamContent(fileStream))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                        try
                        {
                            HttpResponseMessage response = await client.PutAsync(requestUri, content);
                            response.EnsureSuccessStatusCode();
                            success = true;

                            Assert.True(success);
                            _output.WriteLine("File uploaded successfully.");
                        }
                        catch (HttpRequestException e)
                        {
                            _output.WriteLine($"Request error: {e.Message}");
                        }
                        catch (Exception e)
                        {
                            _output.WriteLine($"Unexpected error: {e.Message}");
                        }
                    }
                }
            }
        }
    }
}
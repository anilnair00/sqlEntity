using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals;
using AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable;
using AirCanada.Appx.Common.Enum;
using Csla;
using Csla.Configuration;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AzAiIntegrationEntity = AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable.AzAiIntegration;

namespace AirCanada.Appx.AzAiIntegration.Test
{
    public class ReceiptReaderRequestStateEditTests : IClassFixture<DiCslaFixture>
    {
        private readonly IServiceProvider _serviceProvider;

        public ReceiptReaderRequestStateEditTests(DiCslaFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        [Fact]
        public void FetchAndUpdate_ShouldTransitionStageAndState()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppxDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_FetchAndUpdate")
                .Options;

            using (var context = new AppxDbContext(options))
            {
                context.AzAiIntegrations.Add(new AzAiIntegrationEntity { Id = 95, Stage = "Processing", State = "Queued" });
                context.SaveChanges();
            }

            var services = new ServiceCollection();
            services.AddDbContext<AppxDbContext>(opts => opts.UseInMemoryDatabase("TestDatabase_FetchAndUpdate"));
            services.AddTransient<IReceiptReaderRequestStateEditDal, ReceiptReaderRequestStateEditDal>();
            services.AddCsla();

            // Add a simple console logger for now
            services.AddLogging(builder => builder.AddConsole());

            var serviceProvider = services.BuildServiceProvider();

            // Act
            var dpFactory = serviceProvider.GetRequiredService<IDataPortalFactory>();
            var receiptReaderRequestStateEdit = dpFactory.GetPortal<ReceiptReaderRequestStateEdit>().Fetch(95L);
            receiptReaderRequestStateEdit.Stage = StageEnum.Acknowledgement;
            receiptReaderRequestStateEdit.State = StateEnum.Processed;
            receiptReaderRequestStateEdit = dpFactory.GetPortal<ReceiptReaderRequestStateEdit>().Update(receiptReaderRequestStateEdit);

            // Assert
            using (var context = new AppxDbContext(options))
            {
                var entity = context.AzAiIntegrations.Find(95L);
                entity.Should().NotBeNull();
                if (entity != null)
                {
                    entity.Stage.Should().Be("Acknowledgement");
                    entity.State.Should().Be("Processed");
                }
            }
        }
    }
}
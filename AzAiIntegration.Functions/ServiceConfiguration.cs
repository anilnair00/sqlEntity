
using AirCanada.Appx.AzAiIntegration.DataAccess.ReceiptReader.Dals;
using AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable;
using AirCanada.Appx.AzAiIntegration.Profiles;
using AirCanada.Appx.Claim.DataAccess.Expense.Dals;
using AirCanada.Appx.Common.Wrappers;
using Csla.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;

namespace AirCanada.Appx.AzAiIntegration.Functions
{
    public static class ServiceConfiguration
    {
        public static void AddConfigurations(this IServiceCollection services, HostBuilderContext hostContext)
        {
            services.AddCsla();
            services.AddSingleton<IServiceBusClientWrapper, ServiceBusClientWrapper>();
            services.AddTransient<IAiReceiptReaderRequestDal, AiReceiptReaderRequestDal>();
            services.AddTransient<IExpenseReceiptDocumentDal, ExpenseReceiptDocumentDal>();
            services.AddTransient<IExpenseReceiptDocumentEditDal, ExpenseReceiptDocumentEditDal>();
            services.AddTransient<IReceiptReaderRequestEditDal, ReceiptReaderRequestEditDal>();
            services.AddTransient<IReceiptReaderResponseDal, ReceiptReaderResponseDal>();
            services.AddTransient<IReceiptReaderRequestEditDal, ReceiptReaderRequestEditDal>();
            services.AddTransient<IReceiptReaderRequestStateEditDal, ReceiptReaderRequestStateEditDal>();
            services.AddAutoMapper(typeof(AzAiIntegrationMapper));

            var connectionString = hostContext.Configuration["APPX-ConnectionString"];

            services.AddDbContext<AppxDbContext>(options =>
            options.UseSqlServer(connectionString),
            ServiceLifetime.Scoped);

            var claimConnectionString = hostContext.Configuration["APPX-Claim-ConnectionString"];

            // Inject instances of IDapperWrapper and IDbConnection
            services.AddTransient<IDbConnection>(provider => new SqlConnection(claimConnectionString));
            services.AddTransient<IDapperWrapper, DapperWrapper>();
        }
    }
}
using AirCanada.Appx.Claim.DataAccess.Expense.Dals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Data;

namespace AirCanada.Appx.Claim.DataAccess.Test.Expense.Dals
{
    public class DalTestFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public Mock<IDbConnection> DbConnectionMock { get; private set; }
        public Mock<IDapperWrapper> DapperWrapperMock { get; private set; }

        public DalTestFixture()
        {
            var services = new ServiceCollection();

            // Register the mock IDbConnection
            DbConnectionMock = new Mock<IDbConnection>();
            services.AddSingleton(DbConnectionMock);
            services.AddSingleton(provider => DbConnectionMock.Object);

            // Register the mock IDapperWrapper
            DapperWrapperMock = new Mock<IDapperWrapper>();
            services.AddSingleton(DapperWrapperMock);
            services.AddSingleton(provider => DapperWrapperMock.Object);

            // Register a logger
            services.AddLogging(builder => builder.AddConsole());

            // Add DALs
            services.AddTransient<IExpenseReceiptDocumentEditDal, ExpenseReceiptDocumentEditDal>();

            // Build service provider
            ServiceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        {
            // Dispose of any resources if needed
            (ServiceProvider as IDisposable)?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
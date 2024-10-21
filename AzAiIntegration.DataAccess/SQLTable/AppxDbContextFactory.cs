using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable
{
    public class AppxDbContextFactory : IDesignTimeDbContextFactory<AppxDbContext>
    {
        public AppxDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppxDbContext>();

            var connectionString = Environment.GetEnvironmentVariable("APPX-ConnectionString");

            optionsBuilder.UseSqlServer(connectionString);

            return new AppxDbContext(optionsBuilder.Options);
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable
{
    public class AppxDbContext : DbContext
    {
        public DbSet<AzAiIntegration> AzAiIntegrations { get; set; }

        public AppxDbContext(DbContextOptions<AppxDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WMS_WEBAPI.Data
{
    public class WmsDbContextFactory : IDesignTimeDbContextFactory<WmsDbContext>
    {
        public WmsDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? "Data Source=wms.db";

            var optionsBuilder = new DbContextOptionsBuilder<WmsDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new WmsDbContext(optionsBuilder.Options);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ZLJ.Core;
using ZLJ.Core.Configuration;
using ZLJ.Core.Web;


namespace ZLJ.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class ZLJDbContextFactory : IDesignTimeDbContextFactory<ZLJDbContext>
    {
        public ZLJDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ZLJDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            ZLJDbContextConfigurer.Configure(builder, configuration.GetConnectionString(ZLJ.Core.ZLJConsts.ConnectionStringName));

            return new ZLJDbContext(builder.Options);
        }
    }
}

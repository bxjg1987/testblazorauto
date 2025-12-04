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

            ZLJDbContextConfigurer.Configure(builder, configuration.GetConnectionString(ZLJ.Core.Share.ZLJConsts.ConnectionStringName));

            //由于utils ef项目实现了自动添加业务模块的ef配置，设计时 反射不全，这里手动引用下，以便设计时迁移正常执行
            //BXJG.PSI.MasterData.PSIMasterDataEFModule x;
            //BXJG.Inventory.InventoryEFModule x2;


            return new ZLJDbContext(builder.Options);
        }
    }
}

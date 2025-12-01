using Microsoft.EntityFrameworkCore;

namespace BXJG.PSI.MasterData.EFMaps
{
    public static class ModelBuilderExt
    {
        /// <summary>
        /// 注册PSI主数据模块中的EF映射
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder ApplyConfigurationPSIMasterData(this ModelBuilder modelBuilder)
        {
            return modelBuilder.ApplyConfigurationsFromAssembly(typeof(ModelBuilderExt).Assembly);
        }
    }
}
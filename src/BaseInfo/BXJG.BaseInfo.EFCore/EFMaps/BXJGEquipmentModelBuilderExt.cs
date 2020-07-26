using BXJG.GeneralTree;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BXJG.BaseInfo.EFCore.EFMaps
{
    public static class BXJGBaseInfoModelBuilderExt
    {
        /// <summary>
        /// 配置基础信息模块的EF映射
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder ApplyConfigurationBXJGBaseInfo(this ModelBuilder modelBuilder)
        {
            return modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

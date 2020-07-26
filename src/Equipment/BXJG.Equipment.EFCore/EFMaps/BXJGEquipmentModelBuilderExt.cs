using BXJG.Equipment.EquipmentInfo;
using BXJG.GeneralTree;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BXJG.Equipment
{
    public static class BXJGEquipmentModelBuilderExt
    {
        /// <summary>
        /// 注册设备管理模块的ef映射配置
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder ApplyConfigurationBXJGEquipment(this ModelBuilder modelBuilder)
        {
            return modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

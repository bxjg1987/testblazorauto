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
        public static ModelBuilder ApplyConfigurationBXJGEquipment(this ModelBuilder modelBuilder)
        {
            return modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public static ModelBuilder ApplyConfigurationBXJGEquipment<TDataDictionary>(this ModelBuilder modelBuilder)
        {
            return modelBuilder.ApplyConfiguration(new EquipmentInfoMap<EquipmentInfoEntity<TDataDictionary>, TDataDictionary>())
                               .ApplyConfiguration(new EquipmentInfoMap<TDataDictionary>());
        }
    }
}

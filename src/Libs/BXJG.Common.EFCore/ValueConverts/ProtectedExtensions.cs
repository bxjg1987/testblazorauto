using BXJG.Common.EFCore.ValueConverts;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore
{
    public static class ProtectedExtensions
    {
        /// <summary>
        /// 注册基于Attribute的数据保护
        /// 通常写在OnModelCreating的最后
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="dataProtectionProvider"></param>
        /// <returns></returns>
        public static ModelBuilder ConfigProtect(this ModelBuilder modelBuilder, IDataProtectionProvider dataProtectionProvider)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.PropertyInfo == default)
                        continue;
                    var attributes = property.PropertyInfo.GetCustomAttributes(typeof(ProtectedAttribute), false);
                    if (attributes.Any())
                    {
                        property.SetValueConverter(new ProtectedConverter(dataProtectionProvider));
                    }
                }
            }
            return modelBuilder;
        }
    }
}

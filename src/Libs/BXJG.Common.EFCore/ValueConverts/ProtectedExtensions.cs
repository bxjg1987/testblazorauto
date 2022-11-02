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
        public static ModelBuilder ConfigProtect(this ModelBuilder modelBuilder, IDataProtectionProvider dataProtectionProvider)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
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

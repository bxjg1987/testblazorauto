using Abp.Authorization.Users;
using BXJG.GeneralTree;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.EFCore.EFMaps
{
    public static class BXJGCMSModelBuilderExt
    {
        public static ModelBuilder ApplyConfigurationBXJGCMS(this ModelBuilder modelBuilder)
            //where TUser : AbpUserBase
        {
            return modelBuilder
                .ApplyConfigurationsFromAssembly(BXJGCMSEFCoreModule.GetAssembly())
                .ApplyConfiguration(new ColumnMap())
                .ApplyConfiguration(new ArticleMap());

        }
    }
}

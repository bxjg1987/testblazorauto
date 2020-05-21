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
                .ApplyConfigurationsFromAssembly(BXJGCMSEFCoreModule.GetAssembly());
            //    .ApplyConfiguration(new OrderMap<TUser, TArea>());//上面扫描程序集的方式无法注册带泛型的，所以下面单独加

        }
    }
}

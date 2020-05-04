using Abp.Authorization.Users;
using BXJG.GeneralTree;
using BXJG.Shop.Common;
using BXJG.Shop.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.EFMaps
{
    public class CustomerMap<TUser, TArea> : IEntityTypeConfiguration<CustomerEntity<TUser>>
        where TUser : AbpUserBase
         where TArea : GeneralTreeEntity<TArea>, IShopAdministrative
    {
        public void Configure(EntityTypeBuilder<CustomerEntity<TUser>> builder)
        {
            builder.Property(c => c.RowVersion).IsRowVersion();
        }
    }
}

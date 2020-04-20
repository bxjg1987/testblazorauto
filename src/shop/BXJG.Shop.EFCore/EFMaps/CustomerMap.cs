using Abp.Authorization.Users;
using BXJG.Shop.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.EFMaps
{
    public class CustomerMap<TUser> : IEntityTypeConfiguration<CustomerEntity<TUser>>
        where TUser : AbpUserBase
    {
        public void Configure(EntityTypeBuilder<CustomerEntity<TUser>> builder)
        {
            builder.Property(c => c.RowVersion).IsRowVersion();
        }
    }
}

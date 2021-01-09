using Abp.Authorization.Users;
using BXJG.Common;
using BXJG.GeneralTree;
using BXJG.Shop.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.EFMaps
{
    public class CustomerMap : IEntityTypeConfiguration<CustomerEntity>
    {
        public virtual void Configure(EntityTypeBuilder<CustomerEntity> builder)
        {
            builder.Property(c => c.RowVersion).IsRowVersion();
            builder.Property(c => c.ExtensionData).HasMaxLength(int.MaxValue);
        }
    }
    public class ShippingAddressMap : IEntityTypeConfiguration<ShippingAddressEntity>
    {
        public virtual void Configure(EntityTypeBuilder<ShippingAddressEntity> builder)
        {
            builder.ToTable("BXJGShippingAddress");
            builder.Property(c => c.Name).HasMaxLength(CoreConsts.ShippingAddressNameMaxLength);
            builder.Property(c => c.Phone).HasMaxLength(CoreConsts.ShippingAddressPhoneMaxLength);
            builder.Property(c => c.Address).HasMaxLength(CoreConsts.ShippingAddressAddressMaxLength);
            builder.Property(c => c.ZipCode).HasMaxLength(CoreConsts.ShippingAddressZipCodeMaxLength);
            builder.Property(c => c.ExtensionData).HasMaxLength(int.MaxValue);
        }
    }
}

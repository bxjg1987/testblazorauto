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
    public class CustomerMap<TUser, TArea, TEntity> : IEntityTypeConfiguration<TEntity>
        where TUser : AbpUserBase
        where TArea : GeneralTreeEntity<TArea>, IAdministrative
        where TEntity : CustomerEntity<TUser, TArea>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(c => c.RowVersion).IsRowVersion();
            ///builder.Property(c => c.Birthday).IsRequired(false);
        }
    }
}

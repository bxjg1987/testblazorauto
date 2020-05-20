using Abp.Authorization.Users;
using BXJG.CMS.Ad;
using BXJG.GeneralTree;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.EFCore.EFMaps
{
    public class AdPositionMap : IEntityTypeConfiguration<AdPositionEntity>
    {
        public void Configure(EntityTypeBuilder<AdPositionEntity> builder)
        {
            builder.Property(c => c.DisplayName).IsRequired();
            ///builder.Property(c => c.Birthday).IsRequired(false);
        }
    }
}

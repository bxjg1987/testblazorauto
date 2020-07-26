using BXJG.BaseInfo;
using BXJG.GeneralTree;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.BaseInfo.Administrative;

namespace BXJG.BaseInfo.EFCore.EFMaps
{
    public class AdministrativeMap : IEntityTypeConfiguration<AdministrativeEntity>
    {
        public void Configure(EntityTypeBuilder<AdministrativeEntity> builder)
        {
            //builder.Property(c => c.Title).HasColumnType($"varchar({BXJGShopDictionaryEntity.IconMaxLength})");
            //builder.Property(c => c.Name).HasMaxLength(BXJGBaseInfoConst.BaseInfoInfoNameMaxLength).IsRequired();
            //builder.Property(c => c.Longitude).IsRequired();
            //builder.Property(c => c.Latitude).IsRequired();
        }
    }
}

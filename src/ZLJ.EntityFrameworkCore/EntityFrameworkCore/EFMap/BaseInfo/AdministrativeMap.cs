using ZLJ.Core.BaseInfo;
using BXJG.Utils.GeneralTree;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.Core.BaseInfo.Administrative;

namespace ZLJ.EntityFrameworkCore.EntityFrameworkCore.EFMap.BaseInfo
{
    public class AdministrativeMap : IEntityTypeConfiguration<AdministrativeEntity>
    {
        public void Configure(EntityTypeBuilder<AdministrativeEntity> builder)
        {
            builder.MapGeneralTree().ToTable("baseinfo_administrative");
            //builder.Property(c => c.Title).HasColumnType($"varchar({BXJGShopDictionaryEntity.IconMaxLength})");
            //builder.Property(c => c.Name).HasMaxLength(ZLJ.Core.Share.ZLJConsts.BaseInfoInfoNameMaxLength).IsRequired();
            //builder.Property(c => c.Longitude).IsRequired();
            //builder.Property(c => c.Latitude).IsRequired();
        }
    }
}

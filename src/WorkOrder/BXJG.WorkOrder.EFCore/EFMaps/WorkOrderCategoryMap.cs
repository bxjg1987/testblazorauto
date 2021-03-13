using BXJG.WorkOrder.WorkOrderCategory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.WorkOrder.EFMaps
{
    public class WorkOrderCategoryMap : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            //builder.Property(c => c.Icon).HasColumnType($"varchar({CoreConsts.ItemCategoryIconMaxLength})");
            //builder.Property(c => c.Image1).HasColumnType($"varchar({CoreConsts.ItemCategoryImage1MaxLength})");
            //builder.Property(c => c.Image2).HasColumnType($"varchar({CoreConsts.ItemCategoryImage2MaxLength})");
        }
    }
}

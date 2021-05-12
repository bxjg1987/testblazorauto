using BXJG.WorkOrder.WorkOrderCategory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.EFMaps
{
    public class WorkOrderCategoryTypeMap : IEntityTypeConfiguration<WorkOrderCategoryTypeEntity>
    {
        public void Configure(EntityTypeBuilder<WorkOrderCategoryTypeEntity> builder)
        {
            builder.Property(c => c.WorkOrderType).HasMaxLength(CoreConsts.WorkOrderTypeMaxLength);
            //builder.Property(c => c.Icon).HasColumnType($"varchar({CoreConsts.ItemCategoryIconMaxLength})");
            //builder.Property(c => c.Image1).HasColumnType($"varchar({CoreConsts.ItemCategoryImage1MaxLength})");
            //builder.Property(c => c.Image2).HasColumnType($"varchar({CoreConsts.ItemCategoryImage2MaxLength})");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZLJ.WorkOrder.RentOrderItemWorkOrder;
using BXJG.WorkOrder.EFMaps;
using ZLJ.WorkOrder;
using BXJG.WorkOrder.WorkOrder;

namespace ZLJ.EntityFrameworkCore.EntityFrameworkCore.EFMap.WorkOrder
{
    public class WorkOrderMapBase1 : IEntityTypeConfiguration<OrderBaseEntity>
    {
        public void Configure(EntityTypeBuilder<OrderBaseEntity> builder)
        {
            builder.MapWorkOrderBase()
                   .ToTable("WorkOrder_WorkOrders")
                   .HasDiscriminator(b => b.WorkOrderType);
                   //.Property("Discriminator")
                   //.HasMaxLength(50); 
            //builder.ToTable("WorkOrders", "WorkOrder");
        }
    }
    //public class WorkOrderMapBase2 : IEntityTypeConfiguration<WorkOrderBaseEntity>
    //{
    //    public void Configure(EntityTypeBuilder<WorkOrderBaseEntity> builder)
    //    {
    //        //builder.MapWorkOrderBase();
    //        //builder.ToTable("WorkOrder_WorkOrders");   
    //        //builder.ToTable("WorkOrders", "WorkOrder");
    //    }
    //}

    public class WorkOrderMap : /*WorkOrderBaseMap<RentOrderItemWorkOrderEntity>,*/ IEntityTypeConfiguration<RentOrderItemWorkOrderEntity>
    {
        public  void Configure(EntityTypeBuilder<RentOrderItemWorkOrderEntity> builder)
        {
            //builder.ToTable("WorkOrder_WorkOrderBase" + nameof(RentOrderItemWorkOrderEntity).TrimEnd("Entity".ToCharArray()));
            //builder.ToTable("WorkOrder_WorkOrders");//映射为一样的表，实现TPH，映射到不一样的表是THT，性能不佳
            //base.Configure(builder);
            builder.HasIndex(c => c.UnicodeToken).IsUnique();
            builder.Property(c => c.UnicodeToken).HasColumnType($"varchar({ZLJConsts.UnicodeTokenMaxLength})");
            //builder.Property(c => c.EntityType).HasColumnType($"varchar({CoreConsts.OrderEntityTypeMaxLength})");
            //builder.Property(c => c.EntityId).HasColumnType($"varchar({CoreConsts.OrderEntityIdMaxLength})");
        }
    }
}

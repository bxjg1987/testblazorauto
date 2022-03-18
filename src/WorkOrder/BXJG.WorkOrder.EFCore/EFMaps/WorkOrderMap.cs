using Abp.Authorization.Users;
using BXJG.Common;
using BXJG.Utils.GeneralTree;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using BXJG.WorkOrder.WorkOrder;
namespace BXJG.WorkOrder.EFMaps
{
    /*
     * ef支持多种继承方式，TPH TPT TPC，各自需要不同的映射方式
     * TPH：所有类型公用一张表，根据鉴别列来区分；dbset包含父类
     * TPT：基类一张表，各子类有自己的表，
     * TPC：efcore文档说是不支持，ef6支持的。但实际使用时在efcore中，只有DbSet不配置基类
     */

    /// <summary>
    /// 工单ef映射扩展类
    /// </summary>
    public static class WorkOrderMapExtensions
    {
        /// <summary>
        /// 配置工单抽象类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        public static EntityTypeBuilder<T> MapWorkOrderBase<T>(this EntityTypeBuilder<T> builder) where T : OrderBaseEntity
        {
            //builder.Property(c => c.RowVersion).IsRowVersion();
            //builder.Property(c => c.ExtensionData).HasMaxLength(int.MaxValue);
            builder.Property(c => c.Title).HasMaxLength(CoreConsts.OrderTitleMaxLength).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(CoreConsts.OrderDescriptionMaxLength);
            builder.Property(c => c.StatusChangedDescription).HasMaxLength(CoreConsts.OrderStatusChangedDescriptionMaxLength);
            builder.Property(c => c.EmployeeId).HasColumnType($"varchar({CoreConsts.OrderEmployeeIdMaxLength})");
            //builder.Property(c => c.ContactName).HasMaxLength(CoreConsts.OrderContactNameMaxLength);
            //builder.Property(c => c.ContactPhone).HasColumnType($"varchar({CoreConsts.OrderContactPhoneMaxLength})");
            //外键好像默认会建立索引，但这里没有使用外键
            builder.HasIndex(p => new { p.CategoryId, p.EmployeeId });
            builder.Property(c => c.Status).IsRequired().IsConcurrencyToken();
            builder.Property(c => c.Id).ValueGeneratedNever();
            builder.Property(c => c.WorkOrderType).HasColumnType($"varchar(100)");
            return builder;
        }
    }

    public class WorkOrderBaseMap<T> /*: IEntityTypeConfiguration<T>*/ where T : OrderBaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.MapWorkOrderBase<T>();
            ////builder.Property(c => c.RowVersion).IsRowVersion();
            ////builder.Property(c => c.ExtensionData).HasMaxLength(int.MaxValue);
            //builder.Property(c => c.Title).HasMaxLength(CoreConsts.OrderTitleMaxLength).IsRequired();
            //builder.Property(c => c.Description).HasMaxLength(CoreConsts.OrderDescriptionMaxLength);
            //builder.Property(c => c.StatusChangedDescription).HasMaxLength(CoreConsts.OrderStatusChangedDescriptionMaxLength);
            //builder.Property(c => c.EmployeeId).HasColumnType($"varchar({CoreConsts.OrderEmployeeIdMaxLength})");
            ////builder.Property(c => c.ContactName).HasMaxLength(CoreConsts.OrderContactNameMaxLength);
            ////builder.Property(c => c.ContactPhone).HasColumnType($"varchar({CoreConsts.OrderContactPhoneMaxLength})");
            ////外键好像默认会建立索引，但这里没有使用外键
            //builder.HasIndex(p => new { p.CategoryId, p.EmployeeId });
            //builder.Property(c => c.Status).IsRequired().IsConcurrencyToken();
            //builder.Property(c => c.Id).ValueGeneratedNever();
        }
    }

    public class WorkOrderMap : WorkOrderBaseMap<OrderEntity>, IEntityTypeConfiguration<OrderEntity>
    {
        public override void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            base.Configure(builder);

        }
    }
}
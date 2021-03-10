using Abp.Authorization.Users;
using BXJG.Common;
using BXJG.GeneralTree;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using BXJG.WorkOrder.WorkOrder;
namespace BXJG.WorkOrder.EFMaps
{
    public class WorkOrderBaseMap<T> /*: IEntityTypeConfiguration<T>*/ where T : OrderBaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(c => c.RowVersion).IsRowVersion();
            builder.Property(c => c.ExtensionData).HasMaxLength(int.MaxValue);
            builder.Property(c => c.Title).HasMaxLength(CoreConsts.OrderTitleMaxLength).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(CoreConsts.OrderDescriptionMaxLength);
            builder.Property(c => c.StatusChangedDescription).HasMaxLength(CoreConsts.OrderStatusChangedDescriptionMaxLength);
            builder.Property(c => c.EmployeeId).HasColumnType($"varchar({CoreConsts.OrderEmployeeIdMaxLength})");
            //builder.Property(c => c.ContactName).HasMaxLength(CoreConsts.OrderContactNameMaxLength);
            //builder.Property(c => c.ContactPhone).HasColumnType($"varchar({CoreConsts.OrderContactPhoneMaxLength})");
        }
    }

    public class WorkOrderMap : WorkOrderBaseMap<OrderEntity>, IEntityTypeConfiguration<OrderEntity>
    {

    }
}
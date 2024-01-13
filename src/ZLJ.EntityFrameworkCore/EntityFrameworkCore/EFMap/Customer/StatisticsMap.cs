using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Customer;
using Abp.Authorization.Users;
using Abp.Organizations;

namespace ZLJ.EntityFrameworkCore.EntityFrameworkCore.EFMap.Customer
{
    //public static class _temp
    //{
    //    public static void sdfsdf<T,TKey>(this EntityTypeBuilder<T> builder) where T: PaperUserStatisticsBaseEntity<TKey>
    //    { 
    //        builder.Property(c=>c.Name).
    //    }
    //}

    //public class PaperUseStatisticsOUMap : IEntityTypeConfiguration<PaperUseReportOUEntity>
    //{
    //    public void Configure(EntityTypeBuilder<PaperUseReportOUEntity> builder)
    //    {
    //        builder.ToTable("cust_report_pu_ou");
    //        builder.Property(c => c.ClsName).HasMaxLength(OrganizationUnit.MaxDisplayNameLength);
    //    }
    //}

    //public class PaperUseStatisticsEquipmentMap : IEntityTypeConfiguration<PaperUseReportEquipmentInstanceEntity>
    //{
    //    public void Configure(EntityTypeBuilder<PaperUseReportEquipmentInstanceEntity> builder)
    //    {
    //        builder.ToTable("cust_report_pu_equipment");
    //        builder.Property(c => c.ClsId).HasMaxLength(ZLJ.Core.ZLJConsts.EquipmentInstanceMachineNoMaxLength);
    //        builder.Property(c => c.ClsName).HasMaxLength(ZLJ.Core.ZLJConsts.EquipmentInfoNameMaxLength);
    //    }
    //}
}

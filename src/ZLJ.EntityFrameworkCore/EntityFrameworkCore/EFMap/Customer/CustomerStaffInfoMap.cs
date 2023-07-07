using ZLJ.BaseInfo.StaffInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZLJ.Customer;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace ZLJ.EntityFrameworkCore.EntityFrameworkCore.EFMap.Customer
{
    public class CustomerStaffInfoMap : IEntityTypeConfiguration<CustomerStaffInfoEntity>
    {
        public void Configure(EntityTypeBuilder<CustomerStaffInfoEntity> builder)
        {
            //builder.ToTable("cust_staff"); //¹«ÓÃabpuser±í
            //builder.Property(x => x.Name).HasColumnType($"nvarchar({ZLJConsts.StaffInfoNameMaxLength})").IsRequired();
            //builder.Property(x => x.AgeString).HasMaxLength(ZLJConsts.StaffInfoAgeStringMaxLength);
            //builder.Property(x => x.No).HasMaxLength(ZLJConsts.StaffInfoNoMaxLength);
            //builder.Property(x => x.IdNumber).HasColumnType("varchar").HasMaxLength(ZLJConsts.StaffInfoIdNumberMaxLength);
            //builder.HasIndex(g => g.No).IsUnique();
            //builder.Property(x => x.CurrentAddress).HasColumnType($"nvarchar({ZLJConsts.StaffInfoCurrentAddressMaxLength})");
            // builder.Ignore(c => c.Birthday);
            //builder.Property(x => x.PhoneNumber).HasMaxLength(11);
            builder.HasOne(c => c.Customer).WithMany().OnDelete(DeleteBehavior.NoAction);
            //modelBuilder.Entity<Blog>()
            //.Property(b => b.Url)
            //.HasColumnName("Url");

            //modelBuilder.Entity<RssBlog>()
            //    .Property(b => b.Url)
            //    .HasColumnName("Url");
            //builder.Property(c => c.EquipmentPwd).IsRequired().HasColumnType("varchar").HasMaxLength(ZLJConsts.EquipmentInstancePwdMaxLength);
            builder.Property(c=>c.Birthday).HasColumnName("Birthday");
            builder.Property(c => c.Gender).HasColumnName("Gender");
            //builder.Property(c => c.SyncState).IsConcurrencyToken();
        }
    }
}
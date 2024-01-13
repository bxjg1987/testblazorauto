using ZLJ.Core.BaseInfo.StaffInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ZLJ.EntityFrameworkCore.EntityFrameworkCore.EFMap.BaseInfo
{
    public class StaffInfoMap : IEntityTypeConfiguration<StaffInfoEntity>
    {
        public void Configure(EntityTypeBuilder<StaffInfoEntity> builder)
        {
           // builder.ToTable("baseinfo_staff_info");
            //builder.Property(x => x.Name).HasColumnType($"nvarchar({ZLJ.Core.ZLJConsts.StaffInfoNameMaxLength})").IsRequired();
            //builder.Property(x => x.AgeString).HasMaxLength(ZLJ.Core.ZLJConsts.StaffInfoAgeStringMaxLength);
            builder.Property(x => x.No).HasMaxLength(ZLJ.Core.ZLJConsts.StaffInfoNoMaxLength);
            builder.Property(x => x.IdNumber).HasColumnType("varchar").HasMaxLength(ZLJ.Core.ZLJConsts.StaffInfoIdNumberMaxLength);
            builder.HasIndex(g => g.No).IsUnique();
            builder.Property(x => x.CurrentAddress).HasColumnType($"nvarchar({ZLJ.Core.ZLJConsts.StaffInfoCurrentAddressMaxLength})");

            builder.Property(c => c.Birthday).HasColumnName("Birthday");
            builder.Property(c => c.Gender).HasColumnName("Gender");
            // builder.Ignore(c => c.Birthday);
            //builder.Property(x => x.PhoneNumber).HasMaxLength(11);
        }
    }
}
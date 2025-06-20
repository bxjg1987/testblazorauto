using ZLJ.Core.BaseInfo.AssociatedCompany;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ZLJ.EntityFrameworkCore.EntityFrameworkCore.EFMap.BaseInfo
{
    public class AssociatedCompanyMap : IEntityTypeConfiguration<AssociatedCompanyEntity>
    {
        public void Configure(EntityTypeBuilder<AssociatedCompanyEntity> builder)
        {
            builder.ToTable("baseinfo_associated_company");
            builder.Property(x => x.Name).HasMaxLength(ZLJ.Core.Share.ZLJConsts.AssociatedCompanyNameMaxLength);
            builder.Property(x => x.Pinyin).HasMaxLength(ZLJ.Core.Share.ZLJConsts.AssociatedCompanyNameMaxLength);
            builder.Property(x => x.TaxNo).HasMaxLength(ZLJ.Core.Share.ZLJConsts.AssociatedCompanyTaxNoMaxLength);
            builder.Property(x => x.LinkMan).HasMaxLength(ZLJ.Core.Share.ZLJConsts.AssociatedCompanyLinkManMaxLength);
            builder.Property(x => x.LinkPhone).HasMaxLength(ZLJ.Core.Share.ZLJConsts.AssociatedCompanyLinkPhoneMaxLength);
            builder.Property(x => x.Address).HasMaxLength(ZLJ.Core.Share.ZLJConsts.AssociatedCompanyAddressMaxLength);
            builder.Property(x => x.Lng).HasColumnType($"decimal(32,24)");
            builder.Property(x => x.Lat).HasColumnType($"decimal(32,24)");
            builder.Navigation(x => x.Level).AutoInclude();
            builder.Navigation(x => x.Area).AutoInclude();
            builder.HasOne(a => a.Admin)
       .WithOne()
       .HasForeignKey<AssociatedCompanyEntity>(a => a.AdminId)
       .IsRequired(false)
       .OnDelete(DeleteBehavior.ClientNoAction);  // 删除用户时置空AdminId

            // 用户与客户关系配置
            builder.HasMany(a => a.Staffs)
                .WithOne(s => s.Customer)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
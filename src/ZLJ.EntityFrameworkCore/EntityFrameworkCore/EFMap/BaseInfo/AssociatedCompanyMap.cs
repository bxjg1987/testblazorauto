using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZLJ.Core.AssociatedCompany;

namespace ZLJ.EntityFrameworkCore.EntityFrameworkCore.EFMap.BaseInfo
{
    public class AssociatedCompanyMap : IEntityTypeConfiguration<AssociatedCompanyEntity>
    {
        public void Configure(EntityTypeBuilder<AssociatedCompanyEntity> builder)
        {
            builder.ToTable("KehuXinxi");
            builder.HasIndex(b => new { b.TenantId, b.Name, b.IsDeleted }).IsUnique();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(ZLJ.Core.Share.ZLJConsts.AssociatedCompanyNameMaxLength);
            builder.Property(x => x.Pinyin).HasMaxLength(ZLJ.Core.Share.ZLJConsts.AssociatedCompanyNameMaxLength);
            builder.Property(x => x.TaxNo).HasMaxLength(ZLJ.Core.Share.ZLJConsts.AssociatedCompanyTaxNoMaxLength);
            builder.Property(x => x.LinkMan).HasMaxLength(ZLJ.Core.Share.ZLJConsts.AssociatedCompanyLinkManMaxLength);
            builder.Property(x => x.LinkPhone).HasMaxLength(ZLJ.Core.Share.ZLJConsts.AssociatedCompanyLinkPhoneMaxLength);
            builder.Property(x => x.Address).HasMaxLength(ZLJ.Core.Share.ZLJConsts.AssociatedCompanyAddressMaxLength);

            builder.Property(x => x.Lng).HasColumnType($"decimal(18,6)");
            builder.Property(x => x.Lat).HasColumnType($"decimal(18,6)");
            builder.Navigation(x => x.Level).AutoInclude();
            builder.Navigation(x => x.Area).AutoInclude();
        }
    }
}
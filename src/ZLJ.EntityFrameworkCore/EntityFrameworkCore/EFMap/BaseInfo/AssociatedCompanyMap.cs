using ZLJ.BaseInfo.AssociatedCompany;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ZLJ.EntityFrameworkCore.EntityFrameworkCore.EFMap.BaseInfo
{
    public class AssociatedCompanyMap : IEntityTypeConfiguration<AssociatedCompanyEntity>
    {
        public void Configure(EntityTypeBuilder<AssociatedCompanyEntity> builder)
        {
            builder.ToTable("baseinfo_associated_company");
            builder.Property(x => x.Name).HasMaxLength(ZLJConsts.AssociatedCompanyNameMaxLength);
            builder.Property(x => x.Pinyin).HasMaxLength(ZLJConsts.AssociatedCompanyNameMaxLength);
            builder.Property(x => x.TaxNo).HasMaxLength(ZLJConsts.AssociatedCompanyTaxNoMaxLength);
            builder.Property(x => x.LinkMan).HasMaxLength(ZLJConsts.AssociatedCompanyLinkManMaxLength);
            builder.Property(x => x.LinkPhone).HasMaxLength(ZLJConsts.AssociatedCompanyLinkPhoneMaxLength);
            builder.Property(x => x.Address).HasMaxLength(ZLJConsts.AssociatedCompanyAddressMaxLength);
            builder.Property(x => x.Lng).HasColumnType($"decimal(32,24)");
            builder.Property(x => x.Lat).HasColumnType($"decimal(32,24)");
        }
    }
}
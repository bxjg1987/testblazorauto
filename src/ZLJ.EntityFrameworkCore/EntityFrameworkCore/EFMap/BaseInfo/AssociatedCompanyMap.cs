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
            builder.Property(x => x.Name).HasMaxLength(ZLJ.Core.ZLJConsts.AssociatedCompanyNameMaxLength);
            builder.Property(x => x.Pinyin).HasMaxLength(ZLJ.Core.ZLJConsts.AssociatedCompanyNameMaxLength);
            builder.Property(x => x.TaxNo).HasMaxLength(ZLJ.Core.ZLJConsts.AssociatedCompanyTaxNoMaxLength);
            builder.Property(x => x.LinkMan).HasMaxLength(ZLJ.Core.ZLJConsts.AssociatedCompanyLinkManMaxLength);
            builder.Property(x => x.LinkPhone).HasMaxLength(ZLJ.Core.ZLJConsts.AssociatedCompanyLinkPhoneMaxLength);
            builder.Property(x => x.Address).HasMaxLength(ZLJ.Core.ZLJConsts.AssociatedCompanyAddressMaxLength);
            builder.Property(x => x.Lng).HasColumnType($"decimal(32,24)");
            builder.Property(x => x.Lat).HasColumnType($"decimal(32,24)");
        }
    }
}
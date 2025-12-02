using BXJG.PSI.MasterData.AssociatedCompany;
using BXJG.Utils.Share;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BXJG.PSI.MasterData.EFMaps
{
    public class AssociatedCompanyMap : IEntityTypeConfiguration<BXJGAssociatedCompanyEntity>
    {
        public void Configure(EntityTypeBuilder<BXJGAssociatedCompanyEntity> builder)
        {
            //builder.Metadata.SetDiscriminatorValue("BXJGAssociatedCompany");
            //builder.ToTable("psi_AssociatedCompany", x => x.HasComment("往来单位实体"));
            builder.ToTable("KehuXinxi",x => x.HasComment("往来单位实体"));
            builder.Property(c => c.Id)
                //.ValueGeneratedNever()
                .HasComment("唯一id，主键");
            
            builder.Property(c => c.TenantId)
                .HasComment("租户id");
            
            builder.Property(c => c.ExtensionData)
                .HasMaxLength(BXJGUtilsConsts.ExtDataMaxLength)
                .HasComment("扩展字段");
            
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(BXJGPSIMasterDataCoreConsts.AssociatedCompanyNameMaxLength)
                .HasComment("公司名称");
            
            builder.Property(c => c.Pinyin)
                //.IsRequired()
                .HasMaxLength(BXJGPSIMasterDataCoreConsts.AssociatedCompanyPinyinMaxLength)
                .IsUnicode(false)
                .HasComment("拼音简码");
            
            builder.Property(c => c.IsActive)
                .HasDefaultValue(true)
                .HasComment("是否启用");
            
            builder.Property(c => c.TaxNo)
                .HasMaxLength(BXJGPSIMasterDataCoreConsts.AssociatedCompanyTaxNoMaxLength)
                .IsUnicode(false)
                .HasComment("税号");
            
            builder.Property(c => c.LinkMan)
                .HasMaxLength(BXJGUtilsConsts.PersionNameMaxLength)
                .HasComment("联系人");
            
            builder.Property(c => c.LinkManPinyin)
                .HasMaxLength(BXJGUtilsConsts.PersionNameMaxLength)
                .IsUnicode(false)
                .HasComment("联系人拼音");
            
            builder.Property(c => c.LinkPhone)
                .HasMaxLength(BXJGUtilsConsts.PhoneMaxLength)
                .IsUnicode(false)
                .HasComment("联系电话");
            
            builder.Property(c => c.Address)
                .HasMaxLength(BXJGUtilsConsts.AddressMaxLength)
                .HasComment("详细地址");
            
            builder.Property(c => c.AddressPinyin)
                .HasMaxLength(BXJGUtilsConsts.AddressMaxLength)
                .IsUnicode(false)
                .HasComment("地址拼音");
            
            builder.Property(c => c.Lng)
                .HasPrecision(18, 15)
                .HasComment("经度");
            
            builder.Property(c => c.Lat)
                .HasPrecision(18, 15)
                .HasComment("纬度");
            
            builder.Property(c => c.AreaId)
                .HasComment("所属区域");
            
            builder.Property(c => c.AreaName)
                .HasMaxLength(BXJGUtilsConsts.AreaNameMaxLength)
                .HasComment("所属区域名称");
            
            builder.Property(c => c.ManagerId)
                .HasComment("负责人id");
            
            builder.Property(c => c.ManagerName)
                .HasMaxLength(BXJGUtilsConsts.PersionNameMaxLength)
                .HasComment("负责人姓名");
            
            builder.Property(c => c.LevelId)
                .HasComment("客户等级Id");
            
            builder.Property(c => c.LevelName)
                .HasMaxLength(BXJGUtilsConsts.MaxDisplayNameLength)
                .HasComment("客户等级名称");
            
            builder.HasOne(x => x.Level)
                .WithMany()
                .HasForeignKey(x => x.LevelId);
        }
    }
}
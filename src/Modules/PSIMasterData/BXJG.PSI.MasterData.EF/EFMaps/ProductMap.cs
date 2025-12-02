using BXJG.PSI.MasterData.Product;
using BXJG.Utils.Share;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BXJG.PSI.MasterData.EFMaps
{
    public class ProductMap : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.ToTable("ShebeiXinxi", x => x.HasComment("商品档案实体"));
            
            builder.Property(c => c.Id)
                .HasMaxLength(BXJGUtilsConsts.StringIdMaxLength)
                .IsUnicode(false)
                .HasComment("唯一id，主键");
            
            builder.Property(c => c.TenantId)
                .HasComment("租户id");
            
            builder.Property(c => c.ExtensionData)
                .HasMaxLength(BXJGUtilsConsts.ExtDataMaxLength)
                .HasComment("扩展字段");
            
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(BXJGPSIMasterDataCoreConsts.ProductNameMaxLength)
                .HasComment("商品名称");
            
            builder.Property(c => c.Pinyin)
                .HasMaxLength(BXJGPSIMasterDataCoreConsts.ProductNameMaxLength)
                .IsUnicode(false)
                .HasComment("商品名称拼音简码");
            
            builder.Property(c => c.Model)
                .HasMaxLength(BXJGPSIMasterDataCoreConsts.ProductSpecMaxLength)
                .HasComment("商品规格型号");
            
            builder.Property(c => c.IsVirtual)
                .HasComment("是否是虚拟产品");
            
            builder.Property(c => c.CategoryId)
                //.IsRequired()
                .HasComment("商品类别id");
            
            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Property(c => c.Unit)
                .IsRequired()
                .HasMaxLength(BXJGPSIMasterDataCoreConsts.ProductUnitMaxLength)
                .HasComment("计量单位");
            
            builder.Property(c => c.Remark)
                .HasMaxLength(BXJGUtilsConsts.RemarkMaxLength)
                .HasComment("备注");
            
            builder.Property(c => c.IsActive)
                .HasComment("是否启用");
            
            builder.Property(c => c.OrganizationUnitId)
                .HasComment("所属组织机构id");
            
            builder.HasOne(x => x.OrganizationUnit)
                .WithMany()
                .HasForeignKey(x => x.OrganizationUnitId);
        }
    }
}
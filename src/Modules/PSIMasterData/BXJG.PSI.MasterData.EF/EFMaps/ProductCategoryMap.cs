using BXJG.PSI.MasterData.Product;
using BXJG.Utils.Share;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BXJG.PSI.MasterData.EFMaps
{
    public class ProductCategoryMap : IEntityTypeConfiguration<ProductCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<ProductCategoryEntity> builder)
        {
            builder.ToTable("psi_ProductCategory", x => x.HasComment("商品分类实体"));
            
            // 使用扩展方法配置通用树形结构
            builder.MapGeneralTree();
            
            builder.Property(c => c.ExtensionData)
                .HasMaxLength(BXJGUtilsConsts.ExtDataMaxLength)
                .HasComment("扩展字段");
        }
    }
}
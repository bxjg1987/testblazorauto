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
            builder.ToTable("psi_ProductCategory", x => x.HasComment("产品分类实体"));
            
            builder.Property(c => c.Id)
                .HasComment("唯一id，主键");
            
            builder.Property(c => c.TenantId)
                .HasComment("租户id");
            
            builder.Property(c => c.ExtensionData)
                .HasMaxLength(BXJGUtilsConsts.ExtDataMaxLength)
                .HasComment("扩展字段");
            
            builder.Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(BXJGUtilsConsts.MaxCodeLength)
                .HasComment("分类编码");
            
            builder.Property(c => c.DisplayName)
                .IsRequired()
                .HasMaxLength(BXJGUtilsConsts.MaxDisplayNameLength)
                .HasComment("分类名称");
            
            builder.Property(c => c.ParentId)
                .HasComment("父分类id");
            
            builder.Property(c => c.ChildrenCount)
                .HasComment("子分类数量");
            
            builder.Property(c => c.Name)
                .HasMaxLength(100)
                .HasComment("节点标识");
        }
    }
}
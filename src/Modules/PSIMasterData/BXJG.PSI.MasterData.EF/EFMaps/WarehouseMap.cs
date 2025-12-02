using BXJG.PSI.MasterData.Warehouse;
using BXJG.Utils.Share;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BXJG.PSI.MasterData.EFMaps
{
    public class WarehouseMap : IEntityTypeConfiguration<WarehouseEntity>
    {
        public void Configure(EntityTypeBuilder<WarehouseEntity> builder)
        {
            builder.ToTable("psi_Warehouse", x => x.HasComment("仓库档案实体"));
            
            builder.Property(c => c.Id)
                .ValueGeneratedNever()
                .HasComment("唯一id，主键");
            
            builder.Property(c => c.TenantId)
                .HasComment("租户id");
            
            builder.Property(c => c.ExtensionData)
                .HasMaxLength(BXJGUtilsConsts.ExtDataMaxLength)
                .HasComment("扩展字段");
            
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(BXJGPSIMasterDataCoreConsts.WarehouseNameMaxLength)
                .HasComment("仓库名称");
            
            builder.Property(c => c.Pinyin)
                .HasMaxLength(BXJGUtilsConsts.MaxCodeLength)
                .IsUnicode(false)
                .HasComment("拼音简码");
            
            builder.Property(c => c.IsVirtual)
                .HasComment("是否是虚拟仓库");
            
            builder.Property(c => c.IsPersonal)
                .HasComment("是否是个人仓库");
            
            builder.Property(c => c.AreaId)
                .HasComment("所属省市区县id");
            
            builder.Property(c => c.AreaName)
                .HasMaxLength(BXJGUtilsConsts.AreaNameMaxLength)
                .HasComment("省市区县名称");
            
            builder.Property(c => c.Address)
                .HasMaxLength(BXJGUtilsConsts.AddressMaxLength)
                .HasComment("仓库地址");
            
            builder.Property(c => c.AddressPinyin)
                .HasMaxLength(BXJGUtilsConsts.MaxCodeLength)
                .IsUnicode(false)
                .HasComment("地址拼音简码");
            
            builder.Property(c => c.SquareMeasure)
                .HasComment("面积，㎡");
            
            builder.Property(c => c.Volume)
                .HasComment("体积 m³");
            
            builder.Property(c => c.WarehouseType)
                .HasComment("仓库类型");
            
            builder.Property(c => c.UserId)
                .HasComment("负责人id");
            
            builder.Property(c => c.UserName)
                .HasMaxLength(BXJGUtilsConsts.PersionNameMaxLength)
                .HasComment("负责人姓名");
            
            builder.Property(c => c.Phone)
                .HasMaxLength(BXJGUtilsConsts.PhoneMaxLength)
                .HasComment("联系电话");
            
            builder.Property(c => c.Latitude)
                .HasPrecision(18, 15)
                .HasComment("纬度，用于地理位置定位");
            
            builder.Property(c => c.Longitude)
                .HasPrecision(18, 15)
                .HasComment("经度，用于地理位置定位");
            
            builder.Property(c => c.IsActive)
                .HasDefaultValue(true)
                .HasComment("是否启用");
            
            builder.Property(c => c.OrganizationUnitId)
                .HasComment("所属组织机构id");
            
            builder.Property(c => c.Remark)
                .HasMaxLength(BXJGUtilsConsts.RemarkMaxLength)
                .HasComment("备注");
            
            builder.HasOne(x => x.OrganizationUnit)
                .WithMany()
                .HasForeignKey(x => x.OrganizationUnitId);
        }
    }
}
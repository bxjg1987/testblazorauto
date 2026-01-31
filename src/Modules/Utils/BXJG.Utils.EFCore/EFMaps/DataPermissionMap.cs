using BXJG.Utils.DataPermission;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BXJG.Utils.EFCore.EFMaps
{
    public class DataPermissionMap : IEntityTypeConfiguration<DataPermissionEntity>
    {
        public void Configure(EntityTypeBuilder<DataPermissionEntity> builder)
        {
            builder.HasOne(dp => dp.Metadata)
                   .WithMany()
                   .HasForeignKey(dp => dp.MetaDataId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
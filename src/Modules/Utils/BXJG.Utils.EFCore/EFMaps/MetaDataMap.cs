using BXJG.Utils.GeneralTree;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BXJG.Utils.Metadata;

namespace BXJG.Utils.EFCore.EFMaps
{
    public class MetaDataMap : IEntityTypeConfiguration<MetadataEntity>
    {
        public void Configure(EntityTypeBuilder<MetadataEntity> builder)
        {
            builder.MapGeneralTree();
        }
    }
}

using BXJG.Utils.GeneralTree;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.EFCore.EFMaps
{
    public class MetaDataMap : IEntityTypeConfiguration<Metadata.MetadataEntity>
    {
        public void Configure(EntityTypeBuilder<Metadata.MetadataEntity> builder)
        {
            builder.MapGeneralTree();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo.EFCore.Map
{
    public class GoodsInfoCategoryMap : IEntityTypeConfiguration<GoodsInfoCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<GoodsInfoCategoryEntity> builder)
        {
        }
    }
}

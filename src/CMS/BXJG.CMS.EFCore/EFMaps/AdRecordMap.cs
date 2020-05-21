using Abp.Authorization.Users;
using BXJG.CMS.Ad;
using BXJG.GeneralTree;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.EFCore.EFMaps
{
    public class AdRecordMap : IEntityTypeConfiguration<AdRecordEntity>
    {
        public void Configure(EntityTypeBuilder<AdRecordEntity> builder)
        {
            //builder.Property(c => c.Content).IsRequired();
            ///builder.Property(c => c.Birthday).IsRequired(false);
        }
    }
}

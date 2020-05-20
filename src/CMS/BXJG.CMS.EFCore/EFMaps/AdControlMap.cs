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
    public class AdControlMap : IEntityTypeConfiguration<AdControlEntity>
    {
        public void Configure(EntityTypeBuilder<AdControlEntity> builder)
        {
           // builder.Property(c => c.).IsRowVersion();
            ///builder.Property(c => c.Birthday).IsRequired(false);
        }
    }
}

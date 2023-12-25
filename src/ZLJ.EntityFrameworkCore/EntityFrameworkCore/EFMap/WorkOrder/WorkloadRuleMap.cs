using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.WorkOrder.Workload;

namespace ZLJ.EntityFrameworkCore.EntityFrameworkCore.EFMap.WorkOrder
{
    public class WorkloadRuleMap : IEntityTypeConfiguration<WorkloadRuleEntity>
    {
        public void Configure(EntityTypeBuilder<WorkloadRuleEntity> builder)
        {
            builder.ToTable("WorkOrde.WorkloadRule");
            builder.Property(x => x.RuleParams).HasMaxLength(500);
            builder.Property(x => x.RuleDesc).HasMaxLength(200);
        }
    }
}

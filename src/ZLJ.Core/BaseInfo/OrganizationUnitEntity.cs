using Abp.Domain.Entities;
using Abp.Organizations;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.BaseInfo
{
    public class OrganizationUnitEntity : OrganizationUnit
    {
        public OrganizationUnitEntity() : base() { }
        public OrganizationUnitEntity(int? tenantId, string displayName, long? parentId = null) : base(tenantId, displayName, parentId) { }
        /// <summary>
        /// 0总公司 1分公司 2部门
        /// </summary>
        public Enums.OUType OUType { get; set; }
        [NotMapped]
        public string OUTypeText { get; set; }
    }
}

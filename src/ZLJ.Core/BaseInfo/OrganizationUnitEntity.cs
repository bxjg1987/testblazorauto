using Abp.Domain.Entities;
using Abp.Organizations;
using BXJG.Common.Contracts;
using BXJG.Utils.GeneralTree;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Localization;
using ZLJ.Core.Share;

namespace ZLJ.Core.BaseInfo
{
    public class OrganizationUnitEntity : OrganizationUnit, IGeneralTree<OrganizationUnitEntity>
    {
        public OrganizationUnitEntity() : base() { }
        public OrganizationUnitEntity(int? tenantId, string displayName, long? parentId = null) : base(tenantId, displayName, parentId) { }
        /// <summary>
        /// 0总公司 1分公司 2部门
        /// </summary>
        public OUType OUType { get; set; }
        [NotMapped]
        public string OUTypeText => OUType.Enum();
        [ConcurrencyCheck]
        public int ChildrenCount { get; set; }
        OrganizationUnitEntity IGeneralTree<OrganizationUnitEntity>.Parent
        {
            get
            {
                return base.Parent == default ? default : base.Parent as OrganizationUnitEntity;
            }
            set
            {
                base.Parent = value;
            }
        }
        List<OrganizationUnitEntity> IGeneralTree<OrganizationUnitEntity>.Children
        {
            get { return base.Children.Cast<OrganizationUnitEntity>().ToList(); }
            set { base.Children = value.Cast<OrganizationUnit>().ToList(); }
        }
    }
}

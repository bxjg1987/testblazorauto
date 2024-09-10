using Abp.Domain.Entities;
using Abp.Organizations;
using BXJG.Common.Contracts;
using BXJG.Utils.GeneralTree;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// <summary>
        /// 节点标识，不同租户下同类型的节点，此字段一样
        /// 如：品牌  表示品牌节点，不同租户下此字段值一样
        /// 使用场景：在数据字典功能中，前端下拉框绑定时可以通过此字段绑定指定节点类型
        /// 不能用DisplayName，因为它可能变
        /// 不能用id，因为相同数据库中的不同租户id不同
        /// 不能用code，因为节点移动后，code也会变
        /// 用不到此字段时，请忽略。此字段通常不允许修改
        /// </summary>
        [MaxLength(100)]
        [Unicode(false)]
        [Comment("如：pingpai  表示品牌节点，不同租户下此字段值一样。使用场景：在数据字典功能中，前端下拉框绑定时可以通过此字段绑定指定节点类型")]
        public string? Name { get; set; }
    }
}

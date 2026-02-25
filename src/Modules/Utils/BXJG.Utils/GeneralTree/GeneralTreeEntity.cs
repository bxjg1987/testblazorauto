using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.Common.Contracts;
using BXJG.Utils.Share;
using Castle.MicroKernel.Registration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.GeneralTree
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class GeneralTreeNoTenantEntity<TEntity> : FullAuditedEntity<long>, IExtendableObject, IGeneralTree<TEntity>, IPassivable
        where TEntity : GeneralTreeNoTenantEntity<TEntity>
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; } = true;
        public GeneralTreeNoTenantEntity() { }
        public GeneralTreeNoTenantEntity(string displayName, long? parentId = null)
        {
            this.DisplayName = displayName;
            this.ParentId = parentId;
        }

        [Required]
        [StringLength(BXJG.Utils.Share.BXJGUtilsConsts.MaxCodeLength)]
        [Unicode(false)]
        public virtual string Code { get; set; }

        // public virtual long? ParentId { get; set; }



        long? parentId;




        //[ForeignKey("ParentId")]
        public virtual TEntity Parent { get; set; }


        public virtual List<TEntity> Children { get; set; }
        /// <summary>
        /// 子节点数量
        /// </summary>
        [ConcurrencyCheck]
        public int ChildrenCount { get; set; }

        [Required]
        [StringLength(Share.BXJGUtilsConsts.MaxDisplayNameLength)]
        public virtual string DisplayName { get; set; }

        public string ExtensionData { get; set; }
        public long? ParentId { get => parentId; set => parentId = value; }
        object IHaveParentId.Id { get => Id; set => Id = Convert.ToInt64(value); }
        object IHaveParentId.ParentId { get => ParentId; set => ParentId = value == null ? null : Convert.ToInt64(value); }

        public string GetParentCode()
        {
            if (Parent != null)
                return Parent.Code;
            else
                return Code.Substring(0, Code.Length - Share.BXJGUtilsConsts.CodeUnitLength).TrimEnd('.');
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
        [Comment("如：pinpai  表示品牌节点，不同租户下此字段值一样。使用场景：在数据字典功能中，前端下拉框绑定时可以通过此字段绑定指定节点类型")]
        public string? Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(BXJGUtilsConsts.RemarkMaxLength)]
        public string? Remark { get; set; }
    }

    public abstract class GeneralTreeEntity<TEntity> : GeneralTreeNoTenantEntity<TEntity>, IExtendableObject, IGeneralTree<TEntity>, IPassivable, IMustHaveTenant
       where TEntity : GeneralTreeEntity<TEntity>
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantId { get; set; }
    }

    [Table("BXJGDataDictionaries")]
    public class DataDictionaryEntity : GeneralTreeEntity<DataDictionaryEntity>, IMustHaveTenant
    {
        /// <summary>
        /// 是否是系统预定义的
        /// </summary>
        public bool IsSysDefine { get; set; }
        /// <summary>
        /// 是否是树形的
        /// </summary>
        public bool IsTree { get; set; }
    }
    ///// <summary>
    ///// 行政区实体
    ///// </summary>
    //[Table("BXJGDistricts")]
    //public class DistrictEntity : GeneralTreeEntity<DistrictEntity>
    //{
    //    /// <summary>
    //    /// 主程序的行政级别
    //    /// </summary>
    //    public DistrictLevel Level { get; set; }
    //}
    ///// <summary>
    ///// 行政区级别
    ///// </summary>
    //public enum DistrictLevel
    //{
    //    /// <summary>
    //    /// 省、直辖市
    //    /// </summary>
    //    Province,
    //    /// <summary>
    //    /// 市
    //    /// </summary>
    //    City,
    //    /// <summary>
    //    /// 区县
    //    /// </summary>
    //    County
    //}
}

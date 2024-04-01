using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.Common.Contracts;
using Castle.MicroKernel.Registration;
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
    public abstract class GeneralTreeEntity<TEntity> : FullAuditedEntity<long>, IExtendableObject, IMayHaveTenant, IGeneralTree<TEntity>
        where TEntity : GeneralTreeEntity<TEntity>
    {
       

        public GeneralTreeEntity() { }
        public GeneralTreeEntity(int? tenantId, string displayName, long? parentId = null)
        {
            this.TenantId = tenantId;
            this.DisplayName = displayName;
            this.ParentId = parentId;
        }

        [Required]
        [StringLength(BXJG.Utils.Share.BXJGUtilsConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        // public virtual long? ParentId { get; set; }



        long? parentId;




        //[ForeignKey("ParentId")]
        public virtual TEntity Parent { get; set; }

        public virtual int? TenantId { get; set; }

        public virtual List<TEntity> Children { get; set; }
        /// <summary>
        /// 子节点数量
        /// </summary>
        [ConcurrencyCheck]
        public int ChildrenCount { get; set; }

        [Required]
        [StringLength(Share.BXJGUtilsConsts. MaxDisplayNameLength)]
        public virtual string DisplayName { get; set; }

        public string ExtensionData { get; set; }
        public long? ParentId { get => parentId; set => parentId = value; }
        object IHaveParentId.Id { get => Id; set => Id =   Convert.ToInt64(value); }
        object IHaveParentId.ParentId { get => ParentId; set => ParentId =  value == null ? null : Convert.ToInt64(value); }

        public string GetParentCode()
        {
            if (Parent != null)
                return Parent.Code;
            else
                return Code.Substring(0, Code.Length - Share.BXJGUtilsConsts.CodeUnitLength).TrimEnd('.');
        }
    }

    [Table("BXJGUtilsDataDictionaries")]
    public class DataDictionaryEntity : GeneralTreeEntity<DataDictionaryEntity>
    {
        public bool IsSysDefine { get; set; }
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

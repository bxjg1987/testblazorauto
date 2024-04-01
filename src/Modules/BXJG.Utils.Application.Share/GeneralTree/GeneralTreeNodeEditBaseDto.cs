/*
 * 作者：变形精怪 手机/微信17723896676 QQ/邮箱453008453
 * 创建时间：2018-10-10 23:53:10
 *
 * 说明：略...
 */
using Abp.Application.Services.Dto;
using BXJG.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.GeneralTree
{
    /// <summary>
    /// 树形结构抽象编辑模型
    /// </summary>
    public class GeneralTreeNodeEditBaseDto : EntityDto<long>, IHaveParentId<long>
    {
        /// <summary>
        /// Id
        /// </summary>
        //[Range(0, long.MaxValue)]
        // public long Id { get; set; }
        ///// <summary>
        ///// 父级id
        ///// </summary>
        //public long? ParentId { get; set; }

        long? parentId;
        public long? ParentId { get => parentId; set => parentId = value; }
        object IHaveParentId.Id { get => Id; set => Id = Convert.ToInt64(value); }
        object IHaveParentId.ParentId { get => ParentId; set => ParentId = value == null ? null : Convert.ToInt64(value); }


        /// <summary>
        /// 显示名称
        /// </summary>
        //[Display( Name ="名称", Description ="aaaaa" )] //这些标签对mudblazor无效，但其它blazor ui库也许有用 还有本地化等问题
        //[DisplayName("bbbbb")] //感觉过时了
        [Required(ErrorMessage = "请输入名称")]
        //[StringLength(GeneralTreeEntity.MaxDisplayNameLength)]
        public string DisplayName { get; set; }
        ///// <summary>
        ///// 排序索引
        ///// </summary>
        //public int OrderIndex { get; set; }
        /// <summary>
        /// 扩展属性
        /// </summary>
        public IDictionary<string, object>? ExtData { get; set; } //访问时由于可以配合nameof，对dynamic访问更简单

        /// <summary>
        /// 某些UI框架(如antblazor)，下拉框只能绑定string，所以这里提供个多余的属性
        /// </summary>
        public string? ParentIdString
        {
            get => ParentId.HasValue ? ParentId.ToString() : default;
            set => ParentId = value.IsNotNullOrWhiteSpaceBXJG() ? long.Parse(value) : default;
        }
        //public dynamic ExtData { get; set; }
    }


}

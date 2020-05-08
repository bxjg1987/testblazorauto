using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 上架/商品信息后台管理页面查询列表时使用的输入模型
    /// </summary>
    public class GetAllItemsInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        /// <summary>
        /// 所属商品类别id
        /// </summary>
        public long? CategoryId { get; set; }
        /// <summary>
        /// 品牌Id
        /// </summary>
        public long? BrandId { get; set; }
        /// <summary>
        /// 是否已发布
        /// </summary>
        public bool? Published { get; set; }
        /// <summary>
        /// 关键字
        /// Title/DescriptionShort/SKU等..模糊查询
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 上架时间范围-开始
        /// 注意与实体类中的同名字段意义不同
        /// </summary>
        public DateTimeOffset? AvailableStart { get; set; }
        /// <summary>
        /// 上架时间范围-结束
        /// 注意与实体类中的同名字段意义不同
        /// </summary>
        public DateTimeOffset? AvailableEnd { get; set; }

        public void Normalize()
        {
            if (this.Sorting.IsNullOrEmpty())
                this.Sorting = "creationtime desc";
        }
    }
}

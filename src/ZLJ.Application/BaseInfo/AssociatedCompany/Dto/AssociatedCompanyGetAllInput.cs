using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;

namespace ZLJ.App.Admin.BaseInfo.AssociatedCompany.Dto
{
    /// <summary>
    /// 列表分页查询
    /// </summary>
    public class AssociatedCompanyGetAllInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        /// <summary>
        /// 客户级别，对应数据字典
        /// 20A、21B、22C级...
        /// </summary>
        public long? LevelId { get; set; }
        /// <summary>
        /// 客户类别，对应数据字典
        /// 客户？供应商？即使客户又是供应商？
        /// 16即是客户也是供应商，17供应商，18客户
        /// </summary>
        public long? CategoryId { get; set; }
        /// <summary>
        /// 是否启用 true启用 false禁用
        /// </summary>
        public bool? IsActive { get; set; } = true;
        /// <summary>
        /// 区域编码
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { get; set; }

        [Obsolete("请使用Keywords")]
        public string Q { get; set; }

        /// <summary>
        /// Normalize
        /// </summary>
        public void Normalize()
        {
            if (Sorting.IsNullOrEmpty())
                Sorting = "CreationTime desc";
        }
    }
}
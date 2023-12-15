using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;

namespace ZLJ.App.Customer.StaffInfo
{
    /// <summary>
    /// 列表分页查询
    /// </summary>
    public class GetStaffInfoListInput : GetStaffInfoListCondition, IPagedAndSortedResultRequest, IShouldNormalize
    {
        public int SkipCount { get; set; }
        public int MaxResultCount { get; set; }
        public string Sorting { get; set; }

        /// <summary>
        /// Normalize
        /// </summary>
        public void Normalize()
        {
            if (Sorting.IsNullOrEmpty())
                Sorting = "user.CreationTime desc";
        }
    }
}
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.BaseInfoCommon.StaffInfo
{
    /// <summary>
    /// 获取员工列表时的输入模型
    /// </summary>
    public class GetListInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public GetTotalInput GetTotalInput { get; set; } = new GetTotalInput();

        //public int UserId { get; set; }
        /// <summary>
        /// 所属区域code
        /// </summary>
        public string AreaCode { get; set; }
        /// <summary>
        /// 关键字，模糊查询 姓名、工号、手机号等
        /// </summary>
        public string Keyword { get; set; }
        public virtual void Normalize()
        {
            //if (GetTotalInput == null)
            //    GetTotalInput 

            if (Sorting.IsNullOrEmpty())
                Sorting = "creationtime desc"; //默认最后更新的用户倒叙，因为它可能发生了业务。或者最后登录用户也行

            //if (!Sorting.StartsWith("category", StringComparison.OrdinalIgnoreCase))
            //    Sorting = "order." + Sorting;
            //else
            //    Sorting = Sorting.Replace("category", "category.", StringComparison.OrdinalIgnoreCase);
        }
    }
}

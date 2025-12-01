using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Notifications;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.Notification
{
    /*
     * 不要像之前那样组合GetTotalInput，那样前端传参麻烦
     */

    /// <summary>
    /// 获取通知列表的输入模型
    /// </summary>
    public class GetAllInput : GetTotalInput, IPagedAndSortedResultRequest, IShouldNormalize
    {
        //public GetTotalInput GetTotalInput { get; set; } = new GetTotalInput();
        public int SkipCount { get; set; } = 0;
        public int MaxResultCount { get; set; } = 20;
        public string Sorting { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrEmpty())
                Sorting = "UserNotificationInfo.CreationTime desc, UserNotificationInfo.State asc, TenantNotificationInfo.Severity asc";
        }
    }
}

using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Customer
{
    /// <summary>
    /// 上架/商品信息后台管理页面查询列表时使用的输入模型
    /// </summary>
    public class GetAllCustomersInput : PagedAndSortedResultRequestDto, IShouldNormalize//, ISortedResultRequest 实现它来处理默认排序，CrudAppService 默认是id倒叙
    {
        /// <summary>
        /// 所属区域code，模糊查询
        /// </summary>
        public string AreaCode { get; set; }
        /// <summary>
        /// 姓名、邮箱、手机号等模糊查询
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 模型绑定后，abp会调用此方法来进一步初始化
        /// </summary>
        public void Normalize()
        {
            if (this.Sorting.IsNullOrEmpty())
                this.Sorting = "LastModificationTime desc"; //默认最后更新的用户倒叙，因为它可能发生了业务。或者最后登录用户也行
        }
    }
}

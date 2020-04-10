using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Customer.Dto
{
    /// <summary>
    /// 上架/商品信息后台管理页面查询列表时使用的输入模型
    /// </summary>
    public class GetAllCustomersInput : PagedAndSortedResultRequestDto//, ISortedResultRequest 实现它来处理默认排序，CrudAppService 默认是id倒叙
    {
        public string Keywords { get; set; }
    }
}

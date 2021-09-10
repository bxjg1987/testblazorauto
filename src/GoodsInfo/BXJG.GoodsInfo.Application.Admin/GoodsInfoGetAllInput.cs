using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo.Application.Admin
{
    /// <summary>
    /// 后台管理物品列表页获取数据时的输入模型
    /// </summary>
    public class GoodsInfoGetAllInput<GetGoodsInfoTotalInput> : PagedAndSortedResultRequestDto, IShouldNormalize
    { 
        /// <summary>
      /// 模型绑定后，abp会调用此方法来进一步初始化
      /// </summary>
        public virtual void Normalize()
        {
            if (Sorting.IsNullOrEmpty())
                Sorting = "creationtime desc"; //默认最后更新的用户倒叙，因为它可能发生了业务。或者最后登录用户也行
        }
    }
}

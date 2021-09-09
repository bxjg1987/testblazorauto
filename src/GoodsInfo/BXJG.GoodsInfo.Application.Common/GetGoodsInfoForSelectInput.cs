using Abp.Extensions;
using Abp.Runtime.Validation;
using BXJG.Utils.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo.Application.Common
{
    /// <summary>
    /// 获取物品下拉框数据的输入模型
    /// </summary>
    public class GetGoodsInfoForSelectInput<TGoodsInfoGetTotalInput> : GetPageForSelectInput, IShouldNormalize
        where TGoodsInfoGetTotalInput : GoodsInfoGetTotalInput, new()
    {
        /// <summary>
        /// 条件模型
        /// </summary>
        public TGoodsInfoGetTotalInput GetTotalInput { get; set; } = new TGoodsInfoGetTotalInput();
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

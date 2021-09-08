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
    public class GetGoodsInfoForSelectInput : GetPageForSelectInput
    {
        public string Keywords { get; set; }
    }
}

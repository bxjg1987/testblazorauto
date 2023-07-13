using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Dto
{
    /// <summary>
    /// 获取排序列表时的输入模型
    /// </summary>
    /// <typeparam name="TFilter">条件模型</typeparam>
    public class SortedResultRequest<TFilter> : ISortedResultRequest
    {
        public TFilter Filter { get; set; }
        public string Sorting { get; set; }
    }
}

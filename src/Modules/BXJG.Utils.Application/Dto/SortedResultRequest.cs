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
    public class SortedResultRequest : ISortedResultRequest
    {
        //Abp.Application.Services.Dto.PagedAndSortedResultRequestDto
        //public virtual TFilter Filter { get; set; }
        public virtual string Sorting { get; set; }
    }

    /// <summary>
    /// 获取排序列表时的输入模型
    /// </summary>
    public class SortedResultRequest<TFilter> : SortedResultRequest
    {
        public virtual TFilter Filter { get; set; }
    }
}

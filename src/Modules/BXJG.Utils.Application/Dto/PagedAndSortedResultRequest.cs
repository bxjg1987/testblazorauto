using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Dto
{
    /// <summary>
    /// 抽象的获取分页数据的类
    /// </summary>
    /// <typeparam name="TFilter">条件模型</typeparam>
    public class PagedAndSortedResultRequest<TFilter> : IPagedAndSortedResultRequest
    {
        public TFilter Filter { get; set; }
        PagedAndSortedResultRequestDto pageSortInfo = new PagedAndSortedResultRequestDto();

        public int SkipCount { get => ((IPagedResultRequest)pageSortInfo).SkipCount; set => ((IPagedResultRequest)pageSortInfo).SkipCount = value; }
        public int MaxResultCount { get => ((ILimitedResultRequest)pageSortInfo).MaxResultCount; set => ((ILimitedResultRequest)pageSortInfo).MaxResultCount = value; }
        public string Sorting { get => ((ISortedResultRequest)pageSortInfo).Sorting; set => ((ISortedResultRequest)pageSortInfo).Sorting = value; }
    }
}

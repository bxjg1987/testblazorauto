using Abp.Application.Services.Dto;
using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share
{
    /// <summary>
    /// 将信息作为下拉框数据选择时查询的输入模型
    /// 非分页版本参考<see cref="BXJG.Common.Dto.GetForSelectInput"/>
    /// </summary>
    public class GetPageForSelectInput : GetForSelectInput, IPagedAndSortedResultRequest
    {
        PagedAndSortedResultRequestDto pageSortInfo = new PagedAndSortedResultRequestDto();

        public int SkipCount { get => ((IPagedResultRequest)pageSortInfo).SkipCount; set => ((IPagedResultRequest)pageSortInfo).SkipCount = value; }
        public int MaxResultCount { get => ((ILimitedResultRequest)pageSortInfo).MaxResultCount; set => ((ILimitedResultRequest)pageSortInfo).MaxResultCount = value; }
        public string Sorting { get => ((ISortedResultRequest)pageSortInfo).Sorting; set => ((ISortedResultRequest)pageSortInfo).Sorting = value; }
    }
}

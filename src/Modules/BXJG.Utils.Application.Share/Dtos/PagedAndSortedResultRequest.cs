using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.Dtos
{
    /// <summary>
    /// 获取排序并分页的列表时的输入模型
    /// </summary>
    /// <typeparam name="TFilter">条件模型</typeparam>
    public class PagedAndSortedResultRequest<TFilter> : PagedAndSortedResultRequestDto, IHaveFilter where TFilter : class, new()
    {
        // object _filter;

        public virtual TFilter Filter { get; set; } = new TFilter();
        object IHaveFilter.Filter { get => Filter; set => Filter = value as TFilter; }

        [Range(1, int.MaxValue)]
        public override int MaxResultCount { get; set; } = 20;
        // object IHaveFilter.Filter => this.Filter;
        //public virtual int SkipCount { get; set; }
        //public virtual int MaxResultCount { get; set; }
    }
}
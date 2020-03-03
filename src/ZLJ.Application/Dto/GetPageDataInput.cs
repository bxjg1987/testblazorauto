using Abp.Extensions;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
namespace ZLJ.Dto
{
    /// <summary>
    /// 做easyui分页查询时请求的参数模型
    /// </summary>
    public class GetPageDataInput :  IShouldNormalize//模型验证后abp会调用此接口
    {
        protected IDictionary<string, string> sortFieldMap;

        public GetPageDataInput() { }
        public GetPageDataInput(IDictionary<string, string> sortFieldMap)
        {
            this.sortFieldMap = sortFieldMap;
        }
        /// <summary>
        /// 每页行数
        /// </summary>
        public int Rows { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string Sort { get; set; }
        public string Sorting
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Sort))
                    return Sort;
                string f = Sort;
                if (sortFieldMap != null && sortFieldMap.ContainsKey(f))
                    f = sortFieldMap[f];
                return f + " " + Order;
            }
        }
        /// <summary>
        /// 排序方式 desc或asc
        /// </summary>
        public string Order { get; set; }
        public string Keywords { get; set; }
        // 模型验证后此方法会自动被调用
        public virtual void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sort = "Id";
                Order = "ASC";
                //Sorting = "ExecutionTime DESC";
            }
        }
    }
}

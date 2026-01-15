using BXJG.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.Share.Kehu
{
    public class Condition:IHaveKeywords
    {   /// <summary>
        /// 客户Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 客户级别，对应数据字典
        /// 20A、21B、22C级...
        /// </summary>
        public long? LevelId { get; set; }
        ///// <summary>
        ///// 客户类别，对应数据字典
        ///// 客户？供应商？即使客户又是供应商？
        ///// 16即是客户也是供应商，17供应商，18客户
        ///// </summary>
        //public long? CategoryId { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string? Keywords { get; set; }

        ///// <summary>
        ///// 选中项
        ///// </summary>
        //public long? SelectedId { get; set; }
    }
}

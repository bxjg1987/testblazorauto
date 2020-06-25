using BXJG.Shop.Common.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 批量取消发布商品时的输入模型
    /// </summary>
    public class BatchUnPublishInput: Class1<long>
    {

    }
    /// <summary>
    /// 批量发布商品时的输入模型
    /// </summary>
    public class BatchPublishInput : BatchUnPublishInput
    {
        /// <summary>
        /// 发布开始时间
        /// </summary>
        public DateTimeOffset? AvailableStart { get; set; }
        /// <summary>
        /// 发布结束时间
        /// 与AvailableEndSeconds 二选一
        /// </summary>
        public DateTimeOffset? AvailableEnd { get; set; }
        /// <summary>
        /// 发布结束时间在开始时间之后的多少秒？
        /// 与AvailableEnd 二选一
        /// 若此时不提供AvailableStart，这默认AvailableStart为当前时间并计算结束时间
        /// </summary>
        public long? AvailableEndSeconds { get; set; }
       
    }
}

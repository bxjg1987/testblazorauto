using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.Dto
{
    /// <summary>
    /// 批量操作时的输入模型
    /// </summary>
    public class BulkOperationInput<TKey>
    {
        /// <summary>
        /// 要操作的对象的Id集合
        /// </summary>
        public List<TKey> Ids { get; set; }
    }

    /// <summary>
    /// 批量操作时的输入模型
    /// </summary>
    public class BulkOperationInput: BulkOperationInput<long>
    {
    }
}

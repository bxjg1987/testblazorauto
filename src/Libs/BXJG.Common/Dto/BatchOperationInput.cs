using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Common.Dto
{
    /// <summary>
    /// 批量操作的输入模型
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class BatchOperationInput<TKey>
    {
        public TKey[] Ids { get; set; }
    }
    /// <summary>
    /// 批量操作的输入模型，id类型为int
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class BatchOperationInput : BatchOperationInput<int>
    {
    }

    /// <summary>
    /// 批量操作的输入模型，id类型为long
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class BatchOperationInputLong : BatchOperationInput<long>
    {
    }
}

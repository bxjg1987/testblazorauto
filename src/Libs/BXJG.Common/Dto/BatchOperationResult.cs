using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Common.Dto
{
    public class BatchOperationOutput<TKey> 
    {
        public List<TKey> Ids { get; set; } = new List<TKey>();
    }

    public class BatchOperationOutput : BatchOperationOutput<int>
    {
    }

    /// <summary>
    /// 批量操作的返回模型，id类型为long
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class BatchOperationOutputLong : BatchOperationOutput<long>
    {
    }
}

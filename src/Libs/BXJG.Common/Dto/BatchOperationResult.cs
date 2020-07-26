using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Common.Dto
{
    public class BatchOperationResult<TKey> 
    {
        public List<TKey> Ids { get; set; } = new List<TKey>();
    }

    public class BatchOperationResult : BatchOperationResult<int>
    {
    }

    /// <summary>
    /// 批量操作的返回模型，id类型为long
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class BatchOperationResultLong : BatchOperationResult<long>
    {
    }
}

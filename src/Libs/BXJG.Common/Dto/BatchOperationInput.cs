using System;
using System.Collections.Generic;
using System.Linq;
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


    public class BatchOperationConcurrencyInput<TKey>
        : BatchOperationInput<KeyValuePair<TKey, string>>
    {
        public string this[TKey key]
        {
            get => Ids.Single(c => c.Key.Equals(key)).Value;
        }
    }
    public class BatchOperationConcurrencyInputLong : BatchOperationConcurrencyInput<long>
    {
        //public TKey[] Ids { get; set; }
    }
    public class BatchAuditConcurrencyInput<TKey> : BatchOperationConcurrencyInput<TKey>
    {
        /// <summary>
        /// 是否通过
        /// </summary>
        public bool IsPass { get; set; }
        /// <summary>
        /// 审核意见
        /// </summary>
        public string Reason { get; set; }
    }
    public class BatchAuditConcurrencyInputLong: BatchAuditConcurrencyInput<long>
    {
    }
    public class BatchAuditInput<TKey> : BatchOperationInput<TKey>
    {
        /// <summary>
        /// 是否通过
        /// </summary>
        public bool IsPass { get; set; }
        /// <summary>
        /// 审核意见
        /// </summary>
        public string Reason { get; set; }
    }
    public class BatchAuditInputLong : BatchAuditInput<long>
    {
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

    public class BatchOperationInputString : BatchOperationInput<string>
    {
    }

    public class BatchSwitchInput<TKey> : BatchOperationInput<TKey>
    {
        public bool IsActive { get; set; }
    }
    public class BatchSwitchInput : BatchSwitchInput<int>
    {
    }
    public class BatchSwitchInputLong : BatchSwitchInput<long>
    {
    }
    public class BatchSwitchInputString : BatchSwitchInput<string>
    {
    }
}

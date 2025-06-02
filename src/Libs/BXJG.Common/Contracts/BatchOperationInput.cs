using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXJG.Common.Contracts
{
    #region 批量操作
    /// <summary>
    /// 批量操作的输入模型
    /// </summary>
    /// <typeparam name="TKey">主键/唯一标识的类型</typeparam>
    public class BatchOperationInput<TKey>
    {
        /// <summary>
        /// id数组
        /// </summary>
        public TKey[] Ids { get; set; }
    }

    /// <summary>
    /// 批量操作的输入模型，id类型为int
    /// </summary>
    public class BatchOperationInputInt : BatchOperationInput<int>
    {
    }

    /// <summary>
    /// 批量操作的输入模型，id类型为long
    /// </summary>
    public class BatchOperationInputLong : BatchOperationInput<long>
    {
    }
    /// <summary>
    /// 批量操作的输入模型，id类型为guid
    /// </summary>
    public class BatchOperationInputGuid : BatchOperationInput<Guid>
    {
    }
    /// <summary>
    /// 批量操作的输入模型，id类型为string
    /// </summary>
    public class BatchOperationInputString : BatchOperationInput<string>
    {
    }
    #endregion

    #region 并发
    /// <summary>
    /// 批量操作的输入模型
    /// </summary>
    /// <typeparam name="TKey">id类型</typeparam>
    public class BatchOperationConcurrencyInput<TKey>
        : BatchOperationInput<KeyValuePair<TKey, string>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[TKey key]
        {
            get => Ids.Single(c => c.Key.Equals(key)).Value;
        }
    }
    /// <summary>
    /// 批量操作输入模型，id类型为long
    /// </summary>
    public class BatchOperationConcurrencyInputLong : BatchOperationConcurrencyInput<long>
    {
        //public TKey[] Ids { get; set; }
    }
    /// <summary>
    /// 批量操作输入模型，id类型为guid
    /// </summary>
    public class BatchOperationConcurrencyInputGuid : BatchOperationConcurrencyInput<Guid>
    {
        //public TKey[] Ids { get; set; }
    }
    #endregion

    #region 审核并发

    /// <summary>
    /// 批量审核的输入模型
    /// </summary>
    /// <typeparam name="TKey">id类型</typeparam>
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
    /// <summary>
    /// 批量审核的输入模型，id类型为long
    /// </summary>
    public class BatchAuditConcurrencyInputLong : BatchAuditConcurrencyInput<long>
    {
    }
    /// <summary>
    /// 批量审核的输入模型，id类型为guid
    /// </summary>
    public class BatchAuditConcurrencyInputGuid : BatchAuditConcurrencyInput<Guid>
    {
    }
    #endregion

    #region 审核
    /// <summary>
    /// 批量审核的输入模型
    /// </summary>
    /// <typeparam name="TKey">id类型</typeparam>
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
    /// <summary>
    /// 批量审核的输入模型，id类型为long
    /// </summary>
    public class BatchAuditInputLong : BatchAuditInput<long>
    {
    }
    /// <summary>
    /// 批量审核的输入模型，id类型为guid
    /// </summary>
    public class BatchAuditInputGuid : BatchAuditInput<Guid>
    {
    }
    #endregion

    #region 批量状态变更
    #region 批量状态变更
    /// <summary>
    /// 批量状态调整
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TState"></typeparam>
    public class BatchChangeStateInput<TKey, TState> : BatchOperationInput<TKey>
    {
        public TState State { get; set; }
    }
    /// <summary>
    /// 批量状态调整
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public class BatchChangeStateInputInt<TState> : BatchChangeStateInput<int, TState>
    {
    }
    /// <summary>
    /// 批量状态调整
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public class BatchChangeStateInputString<TState> : BatchChangeStateInput<string, TState>
    {
    }
    /// <summary>
    /// 批量状态调整
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public class BatchChangeStateInputLong<TState> : BatchChangeStateInput<long, TState>
    {
    }
    /// <summary>
    /// 批量状态调整
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public class BatchChangeStateInputGuid<TState> : BatchChangeStateInput<Guid, TState>
    {
    }
    #endregion
    #region 批量开关
    /// <summary>
    /// 批量开关
    /// </summary>
    /// <typeparam name="TKey">id类型</typeparam>
    public class BatchSwitchInput<TKey> : BatchChangeStateInput<TKey, bool>
    {
    }
    /// <summary>
    /// 批量开关输入模型，id类型为int
    /// </summary>
    public class BatchSwitchInputInt : BatchSwitchInput<int>
    {
    }
    /// <summary>
    /// 批量开关输入模型，id类型为int
    /// </summary>
    public class BatchSwitchInputLong : BatchSwitchInput<long>
    {
    }
    /// <summary>
    /// 批量开关输入模型，id类型为guid
    /// </summary>
    public class BatchSwitchInputGuid : BatchSwitchInput<Guid>
    {
    }
    /// <summary>
    /// 批量开关输入模型，id类型为string
    /// </summary>
    public class BatchSwitchInputString : BatchSwitchInput<string>
    {
    }
    #endregion
    #endregion
}

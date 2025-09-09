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

    /*
     * 用户查询某个数据，展示到界面上
     * 然后用户对这些数据进行修改，此时可能另一个用户也查询了数据，也在修改
     * 最后多个用户都提交数据，某些用户的数据会被覆盖，然后界面提示可能是成功的。
     * 
     * 以上情况，传统的乐观锁其实是无法控制。虽然数据最终可能是正确的，
     * 但在用户感官上，数据提交了，也提示成功了，他以为数据就是他提交的状态，但其实已经被替换了，
     * 用户只能在下次查询时才能看到真实保存在数据库中的数据。
     * 
     * 
     * 通常不必考虑这种极端情况，但这里也提供一种思路，就是让乐观锁本身生命周期更长，基本思路如下：
     * 获取数据时返回一个字符串作为乐观锁给前端。
     * 前端提交数据时将乐观锁字符串原样提交回服务端。
     * 保存数据时，在update语句后where条件假设乐观锁是否相等。
     * 
     * 这样A用户在修改时，若B用户也在修改，此时若A用户提交数据后，B用户提交数据时就会直接报错失败，
     * 这样提前将错误告知给用户。
     */

    /// <summary>
    /// 批量操作的输入模型
    /// </summary>
    /// <typeparam name="TKey">id类型</typeparam>
    public class BatchOperationConcurrencyInput<TKey> : BatchOperationInput<KeyValuePair<TKey, string>>
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
    /// <summary>
    /// 批量操作输入模型，id类型为string
    /// </summary>
    public class BatchOperationConcurrencyInputString : BatchOperationConcurrencyInput<string>
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
    /// <summary>
    /// 批量审核的输入模型，id类型为string
    /// </summary>
    public class BatchAuditConcurrencyInputString : BatchAuditConcurrencyInput<string>
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
    /// <summary>
    /// 批量审核的输入模型，id类型为string
    /// </summary>
    public class BatchAuditInputString : BatchAuditInput<string>
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

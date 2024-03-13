using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Common.Dto
{
    /// <summary>
    /// 状态调整
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TState"></typeparam>
    public class ChangeStateInput<TKey, TState> : BatchOperationInput<TKey>
    {
        public TState State { get; set; }
    }
}

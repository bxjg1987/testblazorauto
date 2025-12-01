using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.Application.Share.Dtos
{
    /// <summary>
    /// 状态调整
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TState"></typeparam>
    public class ChangeStateInput<TKey, TState> : EntityDto<TKey>
    {
        public TState State { get; set; }
    }
    /// <summary>
    /// 状态调整
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public class ChangeStateInputInt<TState> : ChangeStateInput<int, TState> { }
    /// <summary>
    /// 状态调整
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public class ChangeStateInputLong<TState> : ChangeStateInput<long, TState> { }
    /// 状态调整
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public class ChangeStateInputGuid<TState> : ChangeStateInput<Guid, TState> { }
    /// 状态调整
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public class ChangeStateInputString<TState> : ChangeStateInput<string, TState> { }


    public class ChangeSwitchInput<TKey> : ChangeStateInput<TKey,bool>  {  }
    public class ChangeSwitchInputInt: ChangeSwitchInput<int> { }
    public class ChangeSwitchInputLong : ChangeSwitchInput<long> { }
    public class ChangeSwitchInputGuid : ChangeSwitchInput<Guid> { }
    public class ChangeSwitchInputString : ChangeSwitchInput<string> { }
}

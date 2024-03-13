using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.Share.Dtos
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
}

using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.Share
{
    /// <summary>
    /// 此接口为下拉或弹窗选择提供数据，abp默认未提供此接口。
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    public interface IProviderBaseAppService<in TGetAllInput, TEntityDto, TKey> : BXJG.Utils.Application.Share.IProviderBaseAppService<TGetAllInput, TEntityDto, TKey>
    {
       
    }
}
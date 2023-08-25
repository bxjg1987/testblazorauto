using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils
{
    /// <summary>
    /// 此接口为下拉或弹窗选择提供数据，abp默认未提供此接口。
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    public interface IProviderBaseAppService<TKey, in TGetAllInput, TEntityDto> : IApplicationService
    {
        /// <summary>
        /// 获取下拉或弹窗的可选分页数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<TEntityDto>> GetAllAsync(TGetAllInput input);
        /// <summary>
        /// 获取关联信息的详情
        /// </summary>
        /// <param name="input">id，之所以没有直接使用TKey，而是包一层是为了跟abp的方式保持统一</param>
        /// <returns></returns>
        Task<TEntityDto> Get(EntityDto<TKey> input);
    }
}

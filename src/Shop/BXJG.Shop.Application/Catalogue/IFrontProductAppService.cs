using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 面向前端顾客关于商品信息的接口
    /// </summary>
    public interface IFrontProductAppService : IApplicationService
    {
        /// <summary>
        /// 搜索商品信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<FrontProductDto>> GetAllAsync(GetAllFrontProductInput input);
    }
}

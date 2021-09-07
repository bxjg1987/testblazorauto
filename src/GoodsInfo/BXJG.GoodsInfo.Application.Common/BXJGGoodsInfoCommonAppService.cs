using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using BXJG.Common.Dto;
using BXJG.Utils.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo.Application.Common
{
    /// <summary>
    /// 公共的物品信息应用服务
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TGetForSelectInput"></typeparam>
    /// <typeparam name="TRepository"></typeparam>
    [AbpAuthorize]
    public class BXJGGoodsInfoCommonAppService<TEntity, TDto, TGetForSelectInput,TRepository> : ApplicationService
        where TEntity : GoodsInfoEntity
        where TDto : GoodsInfoDto
        where TGetForSelectInput : GetForSelectInput
        where TRepository : IGoodsInfoRepository<TEntity>
    {
        private readonly TRepository repository;

        public BXJGGoodsInfoCommonAppService(TRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// 获取物品信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<PagedResultDto<TDto>> GetAllAsync(TGetForSelectInput input)
        {
            throw new NotImplementedException();
        }

        //protected virtual IQueryable<TEntity>
    }
}

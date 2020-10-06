using Abp.Application.Services;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using BXJG.Shop.Authorization;
using Abp.Application.Services.Dto;
using BXJG.Common.Dto;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 商品上架信息应用服务
    /// </summary>
    public interface IProductAppService : IApplicationService
    {
        /// <summary>
        /// 新增商品上架信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.ProductCreate)]
        Task<ProductDto> CreateAsync(ProductUpdateDto input);
        /// <summary>
        /// 修改商品上架信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.ProductUpdate)]
        Task<ProductDto> UpdateAsync(ProductUpdateDto input);
        /// <summary>
        /// 查询商品上架信息的分页数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Product)]
        Task<PagedResultDto<ProductDto>> GetAllAsync(GetAllProductInput input);
        /// <summary>
        /// 根据Id获取商品上架信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Product)]
        Task<ProductDto> GetAsync(EntityDto<long> input);

        //简单起见，目前不用返回值，将来可能包含删除失败的id集合和相应的原因
        //目前不返回，将来添加返回值 对所有调用方也都不影响

        /// <summary>
        /// 批量删除商品上架信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.ProductDelete)]
        Task<BatchOperationResultLong> DeleteAsync(BatchOperationInputLong input);
        /// <summary>
        /// 批量发布商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.ProductUpdate)]//暂时用修改权限，后期补
        Task PublishAsync(BatchPublishInput input);
        /// <summary>
        /// 批量取消发布商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.ProductUpdate)]//暂时用修改权限，后期补
        Task UnPublishAsync(BatchOperationInputLong input);
    }
}

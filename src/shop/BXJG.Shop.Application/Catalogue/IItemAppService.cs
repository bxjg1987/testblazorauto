using Abp.Application.Services;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using BXJG.Shop.Authorization;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 上架信息应用服务
    /// </summary>
    /// <typeparam name="TTree"></typeparam>
    public interface IItemAppService : IApplicationService
    {
        [AbpAuthorize(BXJGShopPermissions.BXJGShopItemCreate)]
        Task<ItemDto> CreateAsync(ItemCreateDto input);
        [AbpAuthorize(BXJGShopPermissions.BXJGShopItemUpdate)]
        Task<ItemDto> UpdateAsync(ItemUpdateDto input);
    }
}

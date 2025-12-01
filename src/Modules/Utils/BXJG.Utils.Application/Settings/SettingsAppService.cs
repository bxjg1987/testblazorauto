using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Domain.Uow;
using BXJG.Utils.Application.Share.Settings;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Settings
{
    /// <summary>
    /// 应用级别（户主修改）和租户级别的设置管理服务
    /// 用户级别的定义在common应用中
    /// </summary>
    //[AbpAuthorize(PermissionNames.AdministratorSystemConfigGet)]
    public abstract class SettingsAppService<TGroupDto, TDto,TEditDto> : BXJGUtilsBaseAppService
        where TGroupDto : SettingDefinitionGroupDto<TGroupDto>,new()
        where TDto : SettingDto<TGroupDto>
        where TEditDto: SettingEditDto
    {
        public ISettingDefinitionManager SettingDefinitionManager { get; set; }

        protected virtual string PermissionNameGet => string.Empty;
        protected virtual string PermissionNameUpdate => string.Empty;
        /*
         * 户主由于不是任何租户，所以只能获取应用级别的设置
         * 
         * 租户不应该看到应用级别的设置，但如果设置的scope包含应用和租户 则也应该返回，因为可能需要覆盖
         * 
         * 用户级别的在common中
         */
        protected virtual  IEnumerable<SettingDefinition> sss() => SettingDefinitionManager.GetAllSettingDefinitions()
                                                                        //若当前是租户登录的，它能处理所有包含租户的
                                                                        .WhereIf(AbpSession.TenantId.HasValue, x => x.Scopes.HasFlag(SettingScopes.Tenant))
                                                                        //若是户主，只要设置定义包含应用范围的，都将被处理
                                                                        .WhereIf(!AbpSession.TenantId.HasValue, x => x.Scopes.HasFlag(SettingScopes.Application));
        /// <summary>
        /// 获取应用和租户级别的设置
        /// </summary>
        /// <returns></returns>
        [UnitOfWork(IsDisabled =true)]
        public virtual List<TGroupDto> GetAllGroups()
        {
            var dgs = sss();
            var dfs = dgs.Where(x=>x.Group!=default).Select(x => x.Group).DistinctBy(x => x.Name);
          
            var dtos = ObjectMapper.Map<List<TGroupDto>>(dfs);
            foreach (var d in dtos)
            {
                d.ChildrenCount = dgs.Where(x => x.Group?.Name == d.Name).Count();
            }
            
            dtos.Add(new TGroupDto
            {
                Name = "",
                DisplayName = "其他",
                ChildrenCount = dgs.Where(x => x.Group == default).Count()
            });
            return dtos;
        }

        /// <summary>
        /// 获取应用和租户级别的设置
        /// </summary>
        /// <param name="input">id对应分组名</param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<List<TDto>> GetSettingByGroup(EntityDto<string> input)
        {
            if (PermissionNameGet.IsNotNullOrWhiteSpaceBXJG())
                await base.PermissionChecker.AuthorizeAsync(PermissionNameGet);
            var sds = sss().Where(x => x.Group?.Name == input.Id||(input.Id.IsNullOrEmpty()&&x.Group==default));

            var svs = await SettingManager.GetAllSettingValuesAsync(SettingScopes.Application | SettingScopes.Tenant);

            var dtos = new List<TDto>();
            foreach (var sd in sds)
            {
                var dto = ObjectMapper.Map<TDto>(sd);
                dto.Group = default;
                dto.Value = svs.FirstOrDefault(x => x.Name == sd.Name)?.Value;
                dtos.Add(dto);
            }
            return dtos;
        }
        /// <summary>
        /// 批量更新设置项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
       // [AbpAuthorize(PermissionNames.AdministratorSystemConfigUpdate)]
        public virtual async Task/*<BatchOperationOutputString>*/ Update(params TEditDto[] input)
        {
            if(PermissionNameUpdate.IsNotNullOrWhiteSpaceBXJG())
             await   base.PermissionChecker.AuthorizeAsync(PermissionNameUpdate);
            var sds = sss();
            //var r = new BatchOperationOutputString();
            foreach (var item in input)
            {
                if (AbpSession.TenantId.HasValue)
                {
                    if (sds.Any(x => x.Name == item.Name && x.Scopes.HasFlag(SettingScopes.Tenant)))
                    {
                        await SettingManager.ChangeSettingForTenantAsync(AbpSession.TenantId.Value, item.Name, item.Value);
                    }
                    else
                        throw new Exception("非法操作");
                }
                else
                {
                    if (sds.Any(x => x.Name == item.Name && x.Scopes.HasFlag(SettingScopes.Application)))
                    {
                        await SettingManager.ChangeSettingForApplicationAsync(item.Name, item.Value);
                    }
                    else
                        throw new Exception("非法操作");
                }
            }
        }
    }

    public class SettingsAppService : SettingsAppService<SettingDefinitionGroupDto, SettingDto, SettingEditDto> {
        protected override string PermissionNameGet => Changliang.PermissionNameGet;
        protected override string PermissionNameUpdate => Changliang.PermissionNameUpdate;
    }
}
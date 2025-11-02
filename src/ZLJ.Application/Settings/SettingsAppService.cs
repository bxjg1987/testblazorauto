using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.Settings;
using ZLJ.Application.Share.Authorization.Permissions;

namespace ZLJ.Application.Settings
{
    /// <summary>
    /// 应用级别（户主修改）和租户级别的设置管理服务
    /// 用户级别的定义在common应用中
    /// </summary>
    [AbpAuthorize(PermissionNames.AdministratorSystemConfigGet)]
    public class SettingsAppService : AdminBaseAppService
    {
        public ISettingDefinitionManager SettingDefinitionManager { get; set; }

        /*
         * 户主由于不是任何租户，所以只能获取应用级别的设置
         * 
         * 租户不应该看到应用级别的设置，但如果设置的scope包含应用和租户 则也应该返回，因为可能需要覆盖
         * 
         * 用户级别的在common中
         */
        IEnumerable<SettingDefinition> sss() => SettingDefinitionManager.GetAllSettingDefinitions()
                                                                        //若当前是租户登录的，它能处理所有包含租户的
                                                                        .WhereIf(AbpSession.TenantId.HasValue, x => x.Scopes.HasFlag(SettingScopes.Tenant))
                                                                        //若是户主，只要设置定义包含应用范围的，都将被处理
                                                                        .WhereIf(!AbpSession.TenantId.HasValue, x => x.Scopes.HasFlag(SettingScopes.Application));
        /// <summary>
        /// 获取应用和租户级别的设置
        /// </summary>
        /// <returns></returns>
        [UnitOfWork(IsDisabled =true)]
        public List<SettingDefinitionGroupDto> GetAllGroups()
        {
            var dgs = sss();
            var dfs = dgs.Where(x=>x.Group!=default).Select(x => x.Group).DistinctBy(x => x.Name);
          
            var dtos = ObjectMapper.Map<List<SettingDefinitionGroupDto>>(dfs);
            foreach (var d in dtos)
            {
                d.ChildrenCount = dgs.Where(x => x.Group?.Name == d.Name).Count();
            }
            
            dtos.Add(new SettingDefinitionGroupDto
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
        public async Task<List<SettingDto>> GetSettingByGroup(EntityDto<string> input)
        {
            var sds = sss().Where(x => x.Group?.Name == input.Id||(input.Id.IsNullOrEmpty()&&x.Group==default));

            var svs = await SettingManager.GetAllSettingValuesAsync(SettingScopes.Application | SettingScopes.Tenant);

            var dtos = new List<SettingDto>();
            foreach (var sd in sds)
            {
                var dto = ObjectMapper.Map<SettingDto>(sd);
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
        [AbpAuthorize(PermissionNames.AdministratorSystemConfigUpdate)]
        public async Task/*<BatchOperationOutputString>*/ Update(params SettingEditDto[] input)
        {
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
                        throw new ApplicationException("非法操作");
                }
                else
                {
                    if (sds.Any(x => x.Name == item.Name && x.Scopes.HasFlag(SettingScopes.Application)))
                    {
                        await SettingManager.ChangeSettingForApplicationAsync(item.Name, item.Value);
                    }
                    else
                        throw new ApplicationException("非法操作");
                }
            }
        }
    }
}
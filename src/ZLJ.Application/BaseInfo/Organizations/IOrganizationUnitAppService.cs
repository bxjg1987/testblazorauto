using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Organizations;
using Abp.Web.Models;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Authorization;
using ZLJ.Organizations.Dto;

namespace ZLJ.Organizations
{
    /// <summary>
    /// 组织单位管理
    /// </summary>
    public interface IOrganizationUnitAppService : IApplicationService
    {
        /// <summary>
        /// 创建树形结构数据的节点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.AdministratorBaseInfoOrganizationUnitAdd)]
        Task<OrganizationUnitDto> CreateAsync(EditOrganizationUnitDto input);
        /// <summary>
        /// 获取所有数据，通常由展示的列表页面调用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.AdministratorBaseInfoOrganizationUnit)]
        Task<IList<OrganizationUnitDto>> GetAllListAsync(GeneralTreeGetTreeInput<long?> input);
        /// <summary>
        /// 获取简洁的树形数据，通常引用此数据的页面调用
        /// </summary>
        /// <param name="input">指定父节点，是否显示全部</param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.AdministratorBaseInfoOrganizationUnit)]
        Task<IList<GeneralTreeNodeDto>> GetTreeForSelectAsync(GeneralTreeGetForSelectInput<long?> input);
        /// <summary>
        /// 获取指定父节点的子节点，以扁平结构返回
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.AdministratorBaseInfoOrganizationUnit)]
        Task<IList<GeneralTreeComboboxDto<long?>>> GetNodesForSelectAsync(GeneralTreeGetForSelectInput<long?> input);
        /// <summary>
        /// 移动节点，服务端将自动重新生成所有兄弟节点的code
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.AdministratorBaseInfoOrganizationUnitUpdate)]
        Task<OrganizationUnitDto> MoveAsync(MoveInput input);
        /// <summary>
        /// 删除指定节点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.AdministratorBaseInfoOrganizationUnitDelete)]
        Task DeleteAsync(EntityDto<long> input);
        /// <summary>
        /// 更新指定节点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.AdministratorBaseInfoOrganizationUnitUpdate)]
        Task<OrganizationUnitDto> UpdateAsync(EditOrganizationUnitDto input);
        /// <summary>
        /// 获取指定节点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.AdministratorBaseInfoOrganizationUnit)]
        Task<OrganizationUnitDto> GetAsync(EntityDto<long> input);
    }
}

using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;

using Abp.AutoMapper;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.UI;
using Abp.Domain.Entities;
using Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace BXJG.GeneralTree
{
    /// <summary>
    /// 数据字典应用服务类
    /// </summary>
    public class UnAuthGeneralTreeAppService : UnAuthGeneralTreeAppServiceBase<GeneralTreeGetForSelectInput,
                                                                               GeneralTreeNodeDto,
                                                                               GeneralTreeGetForSelectInput,
                                                                               GeneralTreeComboboxDto,
                                                                               GeneralTreeEntity>, IUnAuthGeneralTreeAppService
    {
        public UnAuthGeneralTreeAppService(IRepository<GeneralTreeEntity, long> repository) : base(repository)
        { }
    }

    /// <summary>
    /// 数据字典应用服务类
    /// </summary>
    public class GeneralTreeAppService : GeneralTreeAppServiceBase<GeneralTreeDto,
                                                                   GeneralTreeEditDto,
                                                                   GeneralTreeGetTreeInput,
                                                                   GeneralTreeGetForSelectInput,
                                                                   GeneralTreeNodeDto,
                                                                   GeneralTreeGetForSelectInput,
                                                                   GeneralTreeComboboxDto,
                                                                   GeneralTreeNodeMoveInput,
                                                                   GeneralTreeEntity,
                                                                   GeneralTreeManager>, IGeneralTreeAppService
    {
        public GeneralTreeAppService(
            IRepository<GeneralTreeEntity, long> repository,
            GeneralTreeManager organizationUnitManager)
            : base(repository, organizationUnitManager)
        {
            base.createPermissionName = GeneralTreeConsts.GeneralTreeCreatePermissionName;
            base.updatePermissionName = GeneralTreeConsts.GeneralTreeUpdatePermissionName;
            base.deletePermissionName = GeneralTreeConsts.GeneralTreeDeletePermissionName;
            base.getPermissionName = GeneralTreeConsts.GeneralTreeGetPermissionName;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.CheckDeletePermissionAsync();

            var p = await ownRepository.GetAll().AnyAsync(c => c.Id == input.Id && c.IsSysDefine);
            // var sd = await base.AsyncQueryableExecuter.AnyAsync( base.ownRepository.GetAsync(input.Id);
            if (p)
                throw new UserFriendlyException(L("系统预设数据不允许删除！"));

            await base.DeleteAsync(input);
        }
    }
}

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
using BXJG.Common.Dto;

namespace BXJG.Utils.GeneralTree
{
    /// <summary>
    /// 数据字典应用服务类
    /// </summary>
    public class UnAuthGeneralTreeAppService : GeneralTreeProviderBaseAppService<GeneralTreeGetForSelectInput,
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
    public class GeneralTreeAppService : GeneralTreeBaseAppService<GeneralTreeDto,
                                                                   GeneralTreeEditDto,
                                                                   GeneralTreeEditDto,
                                                                   BatchOperationInputLong,
                                                                   GeneralTreeGetTreeInput,
                                                                   EntityDto<long>,
                                                                   GeneralTreeNodeMoveInput,
                                                                   GeneralTreeEntity,
                                                                   GeneralTreeManager>, IGeneralTreeAppService
    {
        public GeneralTreeAppService(IRepository<GeneralTreeEntity, long> repository, 
                                     GeneralTreeManager organizationUnitManager) : base(repository,
                                                                                        organizationUnitManager,
                                                                                        BXJGUtilsConsts.GeneralTreeCreatePermissionName,
                                                                                        BXJGUtilsConsts.GeneralTreeUpdatePermissionName,
                                                                                        BXJGUtilsConsts.GeneralTreeDeletePermissionName,
                                                                                        BXJGUtilsConsts.GeneralTreeMenuName)
        {
        }

        protected override ValueTask BeforeDeleteAsync(GeneralTreeEntity entity)
        {
            if(entity.IsSysDefine)
                throw new UserFriendlyException(L("系统预设数据不允许删除！"));
            return ValueTask.CompletedTask;
        }
    }
}

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
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.GeneralTree;
using BXJG.Utils.Application.Share.Auth;

namespace BXJG.Utils.Application.GeneralTree
{
    /// <summary>
    /// 数据字典应用服务类
    /// </summary>
    public class DataDictionaryProviderAppService : GeneralTreeProviderBaseAppService<GeneralTreeEntity, GeneralTreeGetForSelectInput,
                                                                               GeneralTreeNodeDto,
                                                                               GeneralTreeGetForSelectInput,
                                                                               GeneralTreeComboboxDto>, IDataDictionaryProviderAppService
    {
        //public DataDictionaryProviderAppService(IRepository<GeneralTreeEntity, long> repository) : base(repository)
        //{ }
    }

    /// <summary>
    /// 数据字典应用服务类
    /// </summary>
    public class DataDictionaryAppService : GeneralTreeBaseAppService<GeneralTreeEntity,
                                                                      GeneralTreeDto,
                                                                      GeneralTreeEditDto,
                                                                      GeneralTreeEditDto,
                                                                      GeneralTreeGetTreeInput,
                                                                      BatchOperationInputLong,
                                                                      EntityDto<long>,
                                                                      GeneralTreeNodeMoveInput,
                                                                      GeneralTreeManager>, IDataDictionaryAppService
    {
        public DataDictionaryAppService(IRepository<GeneralTreeEntity, long> repository,
                                     GeneralTreeManager organizationUnitManager) : base(repository,
                                                                                        organizationUnitManager,
                                                                                        PermissionNames.GeneralTreeCreatePermissionName,
                                                                                        PermissionNames.GeneralTreeUpdatePermissionName,
                                                                                        PermissionNames.GeneralTreeDeletePermissionName,
                                                                                        PermissionNames.GeneralTreeMenuName)
        {
        }

        protected override async ValueTask BeforeDeleteAsync(GeneralTreeEntity entity)
        {
            if (entity.IsSysDefine)
                throw new UserFriendlyException(L("系统预设数据不允许删除！"));
            await base.BeforeDeleteAsync(entity);
        }
    }
}

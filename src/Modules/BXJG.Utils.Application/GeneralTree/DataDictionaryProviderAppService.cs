

using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using BXJG.Common.Dto;
using BXJG.Utils.Application.Share.Auth;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.GeneralTree;
using System;
using System.Linq;
using System.Threading.Tasks;

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
                                                                      DataDictionaryGetTreeInput,
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

        protected override IQueryable<GeneralTreeEntity> GetAllFiltered(DataDictionaryGetTreeInput input, string parentCode)
        {
            return base.GetAllFiltered(input, parentCode).WhereIf(input.IsSysDefine.HasValue, x => x.IsSysDefine == input.IsSysDefine.Value)
                                                         .WhereIf(input.Keywords.IsNotNullOrWhiteSpaceBXJG(), x => x.DisplayName.Contains(input.Keywords));
        }
    }
}

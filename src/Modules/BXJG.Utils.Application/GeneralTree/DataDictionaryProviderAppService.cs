

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
    public class DataDictionaryProviderAppService : GeneralTreeProviderBaseAppService<DataDictionaryEntity, GeneralTreeGetForSelectInput,
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
    public class DataDictionaryAppService : GeneralTreeBaseAppService<DataDictionaryEntity,
                                                                      GeneralTreeDto,
                                                                      GeneralTreeEditDto,
                                                                      GeneralTreeEditDto,
                                                                      DataDictionaryGetTreeInput,
                                                                      BatchOperationInputLong,
                                                                      EntityDto<long>,
                                                                      GeneralTreeNodeMoveInput,
                                                                      DataDictionaryManager>, IDataDictionaryAppService
    {
        protected override string CreatePermissionName  => PermissionNames.GeneralTreeCreatePermissionName;
        protected override string UpdatePermissionName => PermissionNames.GeneralTreeUpdatePermissionName;
        protected override string DeletePermissionName => PermissionNames.GeneralTreeDeletePermissionName;
        protected override string GetPermissionName => PermissionNames.GeneralTreeMenuName;

        protected override async ValueTask BeforeDeleteAsync(DataDictionaryEntity entity)
        {
            if (entity.IsSysDefine)
                throw new UserFriendlyException("系统预设数据不允许删除！");
            await base.BeforeDeleteAsync(entity);
        }

        protected override IQueryable<DataDictionaryEntity> GetAllFiltered(DataDictionaryGetTreeInput input, string parentCode)
        {
            return base.GetAllFiltered(input, parentCode).WhereIf(input.IsSysDefine.HasValue, x => x.IsSysDefine == input.IsSysDefine.Value)
                                                         .WhereIf(input.Keywords.IsNotNullOrWhiteSpaceBXJG(), x => x.DisplayName.Contains(input.Keywords));
        }
    }
}

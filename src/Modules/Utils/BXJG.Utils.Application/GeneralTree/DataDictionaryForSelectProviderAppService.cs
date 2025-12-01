

using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using BXJG.Common.Contracts;
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
    public class DataDictionaryForSelectProviderAppService : GeneralTreeProviderBaseAppService<DataDictionaryEntity, GeneralTreeGetForSelectInput,
                                                                               DataDictionaryForSelectDto,
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
                                                                      DataDictionaryDto,
                                                                      DataDictionaryEditDto,
                                                                      DataDictionaryEditDto,
                                                                      DataDictionaryGetTreeInput,
                                                                      DataDictionaryManager>, IDataDictionaryAppService
    {
        protected override string CreatePermissionName  => PermissionNames.GeneralTreeCreatePermissionName;
        protected override string UpdatePermissionName => PermissionNames.GeneralTreeUpdatePermissionName;
        protected override string DeletePermissionName => PermissionNames.GeneralTreeDeletePermissionName;
        protected override string GetPermissionName => PermissionNames.GeneralTreeMenuName;

        protected override async Task DeleteCore(DataDictionaryEntity entity)
        {
            if (entity.IsSysDefine)
                throw new UserFriendlyException("系统预设数据不允许删除！");
            await base.DeleteCore(entity);
        }
        protected override IQueryable<DataDictionaryEntity> GetAllFilter(IQueryable<DataDictionaryEntity> q, DataDictionaryGetTreeInput input, string parentCode)
        {
            return base.GetAllFilter(q,input, parentCode).WhereIf(input.IsSysDefine.HasValue, x => x.IsSysDefine == input.IsSysDefine.Value)
                                                         .WhereIf(input.Keywords.IsNotNullOrWhiteSpaceBXJG(), x => x.DisplayName.Contains(input.Keywords));
        }
    }
}

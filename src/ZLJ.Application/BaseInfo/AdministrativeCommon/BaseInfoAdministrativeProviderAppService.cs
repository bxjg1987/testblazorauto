using Abp.Domain.Repositories;
using BXJG.Utils.GeneralTree;
using Abp.Authorization;
using ZLJ.BaseInfo.Administrative;
using System;

namespace ZLJ.BaseInfo.AdministrativeCommon
{
    [AbpAuthorize]
    [Obsolete("请使用common中的相应服务")]
    public class BaseInfoAdministrativeProviderAppService : UnAuthGeneralTreeAppServiceBase<GeneralTreeGetForSelectInput
        , AdministrativeTreeNodeDto,
        GeneralTreeGetForSelectInput,
        AdministrativeComboboxItemDto,
        AdministrativeEntity>
    {
        public BaseInfoAdministrativeProviderAppService(IRepository<AdministrativeEntity, long> repository) :
            base(repository)
        {
        }
    }
}
using Abp.Domain.Repositories;
using BXJG.Utils.GeneralTree;
using Abp.Authorization;
using ZLJ.Core.BaseInfo.Administrative;
using System;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using BXJG.Common.Dto;
using BXJG.Utils.Enums;
using ZLJ.Application.Common.Administrative;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.Application.GeneralTree;
using BXJG.Utils.Application.Enums;
using ZLJ.Core;

namespace ZLJ.Application.Common.AdministrativeCommon
{
    [AbpAuthorize]
    public class BaseInfoAdministrativeProviderAppService : GeneralTreeProviderBaseAppService<
        AdministrativeEntity,GeneralTreeGetForSelectInput
        , AdministrativeTreeNodeDto,
        GeneralTreeGetForSelectInput,
        AdministrativeComboboxItemDto>
    {
        public BaseInfoAdministrativeProviderAppService(IRepository<AdministrativeEntity, long> repository) :
            base(repository)
        {
            base.LocalizationSourceName = ZLJ.Core.ZLJConsts.LocalizationSourceName;
        }

        /// <summary>
        /// 获取行政区域级别下拉数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<ComboboxItemDto> GetAdministrativeLevels(GetForSelectInput input)
        {
            return new EnumManager(LocalizationSource).ConvertToComboboxData<AdministrativeLevel>(input);
        }
    }
}
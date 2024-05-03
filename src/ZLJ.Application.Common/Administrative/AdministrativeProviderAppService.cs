using Abp.Domain.Repositories;
using BXJG.Utils.GeneralTree;
using Abp.Authorization;
using System;
using Abp.Application.Services.Dto;
using System.Collections.Generic;

using BXJG.Utils.Enums;
using ZLJ.Application.Common.Administrative;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.Application.GeneralTree;
using BXJG.Utils.Application.Enums;
using ZLJ.Core;
using ZLJ.Core.Administrative;
using BXJG.Common.Contracts;
using ZLJ.Core.Share.Enums;

namespace ZLJ.Application.Common.AdministrativeCommon
{
    [AbpAuthorize]
    public class AdministrativeProviderAppService : CommonTreeProviderBaseAppService<
        AdministrativeEntity,GeneralTreeGetForSelectInput
        , AdministrativeTreeNodeDto,
        GeneralTreeGetForSelectInput,
        AdministrativeComboboxItemDto>
    {
        public AdministrativeProviderAppService(IRepository<AdministrativeEntity, long> repository)
         //  : base(repository)
        {
            base.LocalizationSourceName = ZLJ.Core.Share.ZLJConsts.LocalizationSourceName;
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
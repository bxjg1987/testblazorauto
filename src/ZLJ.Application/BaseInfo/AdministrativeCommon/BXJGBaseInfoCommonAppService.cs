using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ZLJ.Localization;
using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using BXJG.Utils;
using ZLJ.BaseInfo.Administrative;
using BXJG.Utils.Enums;

namespace ZLJ.BaseInfo.AdministrativeCommon
{
    public class BXJGBaseInfoCommonAppService : ApplicationService
    {
        public BXJGBaseInfoCommonAppService() {
            base.LocalizationSourceName = ZLJConsts.LocalizationSourceName;
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
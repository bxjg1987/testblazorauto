using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BXJG.Utils.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using BXJG.Utils.Localization;
using BXJG.Utils.Enums;

namespace BXJG.CMS.Common
{
    public class BXJGCMSCommonAppService : ApplicationService, IBXJGCMSCommonAppService
    {
        public BXJGCMSCommonAppService()
        {
            base.LocalizationSourceName = BXJGCMSConsts.LocalizationSourceName;
        }

        public List<ComboboxItemDto> GetColumnTypes(GetForSelectInput input)
        {
            return new EnumManager(base.LocalizationSource).GetAllOrderStatus<Column.ColumnType>(input);
        }
    }
}

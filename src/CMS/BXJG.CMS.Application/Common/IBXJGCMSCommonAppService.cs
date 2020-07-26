using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Common
{
    public interface IBXJGCMSCommonAppService : IApplicationService
    {
        List<ComboboxItemDto> GetColumnTypes(GetForSelectInput input);
    }
}

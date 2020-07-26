using Abp.Application.Services.Dto;
using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.BaseInfo
{
    /// <summary>
    /// 一些辅助接口，有的需要登录才能访问，有的不需要登陆就可以访问
    /// </summary>
    public interface IBXJGBaseInfoCommonAppService
    {
        public List<ComboboxItemDto> GetAdministrativeLevels(GetForSelectInput input);
    }
}

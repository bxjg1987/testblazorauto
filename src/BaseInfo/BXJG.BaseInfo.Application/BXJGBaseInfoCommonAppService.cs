using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BXJG.BaseInfo.Localization;
using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.BaseInfo.Administrative;

namespace BXJG.BaseInfo
{
    public class BXJGBaseInfoCommonAppService : ApplicationService, IBXJGBaseInfoCommonAppService
    {
        private readonly EnumManagerFactory enumManagerFactory;

        public BXJGBaseInfoCommonAppService(EnumManagerFactory enumManagerFactory)
        {
            this.enumManagerFactory = enumManagerFactory;
        }
        /// <summary>
        /// 获取行政区域级别下拉数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<ComboboxItemDto> GetAdministrativeLevels(GetForSelectInput input)
        {
            return enumManagerFactory.EnumManager.GetAllOrderStatus<AdministrativeLevel>(input);
        }
    }
}

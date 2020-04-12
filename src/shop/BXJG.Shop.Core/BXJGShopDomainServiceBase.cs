using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop
{
    /// <summary>
    /// 商城系统 领域服务基类
    /// </summary>
    public class BXJGShopDomainServiceBase : DomainService
    {
        public BXJGShopDomainServiceBase() {
            base.LocalizationSourceName = BXJGShopConsts.LocalizationSourceName;
        }
    }
}

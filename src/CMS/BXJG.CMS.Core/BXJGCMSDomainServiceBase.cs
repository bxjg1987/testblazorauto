using Abp.Domain.Services;
using Abp.Events.Bus;
using Abp.Threading;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS
{
    /// <summary>
    /// CMS系统 领域服务基类
    /// </summary>
    public class BXJGCMSDomainServiceBase : DomainService
    {
        public  IEventBus EventBus { get;}= NullEventBus.Instance;//空模式

        public BXJGCMSDomainServiceBase()
        {
            base.LocalizationSourceName = BXJGCMSConsts.LocalizationSourceName;
        }
    }
}

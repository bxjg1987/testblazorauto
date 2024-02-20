using Abp.Domain.Services;
using Abp.Events.Bus;
using Abp.Linq;
using Abp.Runtime.Session;
using Abp.Threading;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZLJ.Core
{
    /// <summary>
    /// 领域服务基类
    /// </summary>
    public class BXJGBaseInfoDomainServiceBase : DomainService
    {
        //参考 https://github.com/aspnetboilerplate/aspnetboilerplate/blob/c2c2e4fee2a680e5e2baa196defa774e4d3d68f2/src/Abp/Domain/Repositories/AbpRepositoryBase.cs
        //运行时将属性注入一个基于HttpContext的实现，意思是请求中断时会直接取消整个请求中所有的异步，前提是这些异步操作都传入了CancellationToken
        //调用方若想使用自己的CancelToken可以使用USE
        //由于大部异步操作都是基于本地数据库的，因此只有特殊场景才需要这个，所以抽象类中不要定义这个
        //public ICancellationTokenProvider CancellationToken { get; set; } = NullCancellationTokenProvider.Instance;

        public IAbpSession AbpSession { get; set; } = NullAbpSession.Instance;
        public IEventBus EventBus { get; set; } = NullEventBus.Instance;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;

        public BXJGBaseInfoDomainServiceBase()
        {
            base.LocalizationSourceName = ZLJ.Core.Share.ZLJConsts.LocalizationSourceName;
            
            //空模式
            //EventBus = NullEventBus.Instance;
            //AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }
    }
}

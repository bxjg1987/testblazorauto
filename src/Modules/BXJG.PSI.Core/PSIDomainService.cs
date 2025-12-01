using Abp.Domain.Services;
using Abp.Linq;
using Abp.Threading;
using BXJG.Utils;
using BXJG.Utils.Share;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.PSI
{
    /// <summary>
    /// 进销存模块领域服务基类，提供多种方式允许调用方实现自己的逻辑
    /// 模块使用方往往有自己的继承结构，它应该引用此服务，而不是继承它。
    /// 如果需要替换里面的部分逻辑，提供自己的子类，然后替换此服务，但最终还是建议使用组合的方式使用它。
    /// 为了进一步简化调用方的操作，某些方法还提供了传入委托，以便自定义指定步骤的执行逻辑。
    /// 也可以使用abp提供的事件方式
    /// </summary>
    public abstract class PSIDomainService : BXJGBaseDomainService
    {
        //本项目中严重依赖ef，以便更方便地操作数据库
        //所以建议不要使用下面这个属性
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;

        public IHostEnvironment HostEnvironment { get; set; }
        public ICancellationTokenProvider CancellationTokenProvider { get; set; }

        public PSIDomainService()
        {
            base.LocalizationSourceName = BXJGPSICoreConsts.LocalizationSourceName;
        }
    }
}
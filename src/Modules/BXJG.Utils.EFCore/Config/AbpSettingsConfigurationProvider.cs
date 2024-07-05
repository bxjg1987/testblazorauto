using BXJG.Common.Events;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.EFCore.Config
{
    //反正内部要使用ef，可以考虑使用ef的事件来通知。

    public class AbpSettingsConfigurationProvider : ConfigurationProvider, IDisposable
    {
        IDisposable d;
        public AbpSettingsConfigurationProvider()
        {
            d = Zhongjie.Instance.Zhuce(() =>
            {
                Load();
                OnReload();
                return ValueTask.CompletedTask;
            }, "OnAbpApplicationSettingsChanged");
        }

        public void Dispose()
        {
            d?.Dispose();
        }

        // IConfigurationBuilder

        public override void Load()
        {
            
            //直接从数据库加载应用程序配置,
            //反正提供器好像不容易使用依赖注入，所以这里直接new dbcontext即可，记得释放
            //或者这里使用ef的事件实现
        }
    }
}
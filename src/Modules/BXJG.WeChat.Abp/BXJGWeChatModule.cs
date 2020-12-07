using Abp.Modules;
using System;
using System.Reflection;

namespace BXJG.WeChat
{
    public class BXJGWeChatModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

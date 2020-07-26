using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.BaseInfo;
using System;
using System.Reflection;

namespace BXJG.BaseInfo.EFCore
{
    public class BXJGBaseInfoEFCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

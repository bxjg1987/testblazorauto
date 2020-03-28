using Abp.Dependency;
using BXJG.WeChat.MiniProgram;
using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BXJG.WeChat.ABP
{
    public class BXJGConventionalDependencyRegistrar : IConventionalDependencyRegistrar
    {
        public void RegisterAssembly(IConventionalRegistrationContext context)
        {
            context.IocManager.IocContainer.Register(
                 Classes.FromAssembly(context.Assembly)
                     .BasedOn<IWeChatMiniProgramLoginHandler>()
                     .WithServiceFromInterface(typeof(IWeChatMiniProgramLoginHandler))
                     //.If(type => !type.GetTypeInfo().IsGenericTypeDefinition)
                     .LifestyleTransient()
                 );
        }
    }
}

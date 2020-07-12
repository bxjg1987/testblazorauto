using Abp.Dependency;
using BXJG.Utils.File;
using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BXJG.Utils
{
    public class Class1 : IConventionalDependencyRegistrar
    {
        public void RegisterAssembly(IConventionalRegistrationContext context)
        {
            //Transient
            context.IocManager.IocContainer.Register(
                Classes.FromAssembly(context.Assembly)
                    .IncludeNonPublicTypes()
                    .BasedOn<IEnv>()
                    .If(type => !type.GetTypeInfo().IsGenericTypeDefinition)
                    .WithService.AllInterfaces()
                    .WithService.DefaultInterfaces()
                    .LifestyleSingleton()
                );
        }
    }
}

using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp;
using BXJG.Utils.Localization;
using BXJG.Utils.Enums;
using Abp.Threading.BackgroundWorkers;
using BXJG.Utils.Files;
using BXJG.Common;
using Abp.Dependency;
using BXJG.Utils.DynamicProperty;
using Abp.AutoMapper;
using System.Reflection;
using Abp.Domain.Entities;
using System.Collections.Generic;
using AutoMapper;
using BXJG.Common.Dto;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Abp.Domain.Uow;
using System;
using Abp.Threading;
using System.Threading.Tasks;
using BXJG.Utils.GeneralTree;
using Microsoft.Extensions.DependencyInjection;

namespace BXJG.Utils
{
    /*
     * 通用公共功能模块
     */
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class BXJGUtilsModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<BXJGUtilsModuleConfig>();
            //Configuration.Modules.BXJGUtils().AddEnum(typeof(Gender), "gender", UtilsConsts.LocalizationSourceName);
            Configuration.Modules.BXJGUtils().EnumLocalizationProviders.Add(() => new[] {
                new EnumLocalizationDefine(typeof(Gender), "gender"),
                new EnumLocalizationDefine(typeof(Gender), "bool"),
            });
            BXJGUtilsLocalizationConfigurer.Configure(Configuration.Localization);
            Configuration.Settings.Providers.Add<BXJGUtilsSettingProvider>();
            //查看abp源码 uow拦截器调用manager.begin，内部从ioc获取uow对象efuow 然后设置到asynclocal上的
            IocManager.IocContainer.Kernel.ComponentCreated += Kernel_ComponentCreated;
        }

        private void Kernel_ComponentCreated(Castle.Core.ComponentModel model, object instance)
        {
            if (instance is IActiveUnitOfWork uow)
            {
                uow.Disposed -= Uow_Disposed;//保险起见
                uow.Disposed += Uow_Disposed;
            }
        }

        private static void Uow_Disposed(object sender, EventArgs e)
        {
            //参考ActiveUnitOfWorkExtensions
            var uow = sender as IActiveUnitOfWork;
            if (uow.Items.TryGetValue(ActiveUnitOfWorkExtensions.__disposeableObject, out var t))
            {
                foreach (var item in (t as HashSet<object>))
                {
                    if (item is IAsyncDisposable d)
                        AsyncHelper.RunSync(() => d.DisposeAsync().AsTask());

                    if (item is IDisposable ee)
                        ee.Dispose();
                }
            }
        }

        public override void Initialize()
        {
            //注册非abp依赖的公共库Common中的服务
            //base.IocManager.RegisterIfNot<IClock, LocalClock>(DependencyLifeStyle.Singleton);
            //base.IocManager.RegisterIfNot<IEnv,BXJG.Common.DefaultWebEnv>(DependencyLifeStyle.Singleton);


            IocManager.RegisterAssemblyByConvention(typeof(BXJGUtilsModule).GetAssembly());
            //IocManager.Register<IClock, AbpClock>();
            IocManager.RegService(services => services.AddBXJGCommon());
            IocManager.Register(typeof(DynamicPropertyManager<>), DependencyLifeStyle.Singleton);

            //IocManager.Register(typeof(AttachmentManager<>), DependencyLifeStyle.Transient);
            //调试模式时默认实现获取的路径是 ..\bin\debug\wwwroot
            //而asp.net core默认读取是在ZLJ.Web.Host\wwwroot 导致上传的文件看不到效果
            //发布到服务器后不存在这个问题，调试时需要在web.core模块PreInitialize中替换服务，注意经过测试一定要在PreInitialize中替换
            //IocManager.Register<IEnv, Utils.File.DefaultEnv>(Abp.Dependency.DependencyLifeStyle.Singleton);

            IocManager.Register<IEnv, NullEnv>();
            IocManager.Register(typeof(GeneralTreeManager<>), DependencyLifeStyle.Transient);

        }

        public override void PostInitialize()
        {
            #region 本地化枚举系统
            var utilsCfg = Configuration.Modules.BXJGUtils();
            var list = new List<EnumLocalizationDefine>();
            foreach (var item in utilsCfg.EnumLocalizationProviders)
            {
                var items = item();

                foreach (var item2 in items)
                {
                    var temp = list.SingleOrDefault(c => c.Name == item2.Name);
                    if (temp != null)
                    {
                        list.Remove(temp);
                    }
                    list.Add(item2);
                }
            }
            utilsCfg.EnumLocalizationProviders = null;
            var sdf = new EnumLocalizationContainer(list);
            base.IocManager.RegService(c => c.TryAddSingleton(sdf));
            #endregion




            //运行ZLJ.Migrator时会确实依赖服务，所以这里try下 但是这里try了 若正式用的时候你都不晓得错没错
            // try
            //{

            //Task.Yield();
            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workManager.Add(IocManager.Resolve<RemoveUploadFileWorker>());
            //}
            // catch
            // {

            //}
        }
    }
}

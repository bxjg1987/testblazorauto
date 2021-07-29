using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.BaseInfo;
//using BXJG.DynamicAssociateEntity;
using BXJG.Equipment;
using BXJG.GeneralTree;
using BXJG.Shop;
using BXJG.Shop.Catalogue;
using BXJG.WorkOrder;
using System.Collections.Generic;
using System.Reflection;

using ZLJ.Authorization;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
//using ZLJ.DynamicAssociateEntity;
using ZLJ.Localization;
using ZLJ.MultiTenancy;

namespace ZLJ
{
    [DependsOn(
        typeof(ZLJCoreModule),
        typeof(AbpAutoMapperModule),
        typeof(BXJGEquipmentApplicationModule),
        typeof(BXJGBaseInfoApplicationModule))]
    public class ZLJApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<ZLJAuthorizationProvider>();
            //Configuration.Modules.DynamicAssociateEntity().DynamicAssociateEntityDefineProviders = DynamicAssociateEntityConfig.GetDefines;
            //Configuration.Modules.DynamicAssociateEntity().DynamicAssociateEntityDefineGroupProvider = DynamicAssociateEntityConfig.DynamicAssociateEntityMap;
        }

        public override void Initialize()
        {
            //经过测试，这样abp还是无法生成动态webapi，手动提供实现类吧
            //IocManager.Register(typeof(IBXJGShopItemAppService), typeof(BXJGShopItemAppService<Tenant, User, Role, TenantManager, UserManager, GeneralTreeEntity>), DependencyLifeStyle.Transient);
            //IocManager.Register(typeof(IBXJGShopFrontItemAppService), typeof(BXJGShopFrontItemAppService<GeneralTreeEntity>), DependencyLifeStyle.Transient);
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            //IocManager.RegisterBXJGWorkOrderDefaultAdapter<User>();
            //IocManager.Register<IEmployeeAppService, WorkOrder.EmployeeAppService>(DependencyLifeStyle.Transient);
            //IocManager.Register<IEmployeeSession, EmployeeSession>(DependencyLifeStyle.Transient);

            //注册automapper映射
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
            //Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg => cfg.AddProfile<BXJGShopMapProfile<User>>());
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg => cfg.AddProfile<BXJGEquipmentMapProfile<User>>());
        }
    }
}

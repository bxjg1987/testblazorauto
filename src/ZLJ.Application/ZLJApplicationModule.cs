using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.GeneralTree;
using BXJG.Shop;
using BXJG.Shop.Catalogue;
using System.Reflection;
using ZLJ.Administrative;
using ZLJ.Authorization;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;

namespace ZLJ
{
    [DependsOn(
        typeof(ZLJCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class ZLJApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<ZLJAuthorizationProvider>();
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg => cfg.AddProfile<BXJGShopMapProfile<User, AdministrativeEntity, GeneralTreeEntity>>());
        }

        public override void Initialize()
        {

            IocManager.RegisterAssemblyByConvention(typeof(ZLJApplicationModule).GetAssembly());

            //经过测试，这样abp还是无法生成动态webapi，手动提供实现类吧
            //IocManager.Register(typeof(IBXJGShopItemAppService), typeof(BXJGShopItemAppService<Tenant, User, Role, TenantManager, UserManager, GeneralTreeEntity>), DependencyLifeStyle.Transient);
            //IocManager.Register(typeof(IBXJGShopFrontItemAppService), typeof(BXJGShopFrontItemAppService<GeneralTreeEntity>), DependencyLifeStyle.Transient);
        }
    }
}

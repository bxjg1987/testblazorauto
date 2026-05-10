using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.Threading;
using BXJG.Utils.Auth;
using BXJG.Utils.Enums;
using BXJG.Utils.Extensions;
using BXJG.Utils.Share.Tag;
using BXJG.Utils.Tag;
using Microsoft.EntityFrameworkCore;


//using DotNetCore.CAP;
//using DotNetCore.CAP.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils
{
    public class BXJGUtilsModuleConfig
    {
        //EnumLocalizationContainer enumLocalizationContainer;

        //public BXJGUtilsModuleConfig(EnumLocalizationContainer enumLocalizationContainer)
        //{
        //    this.enumLocalizationContainer = enumLocalizationContainer;
        //}

        ////最终处理办法是属性类型直接用List 而不是IList
        //[Obsolete("需要访问此对象时应直接注入EnumLocalizationContainer")]
        //public IReadOnlyList<EnumLocalizationDefine> Enums => enumLocalizationContainer;

        /// <summary>
        /// 通过它注册本地化枚举定义
        /// </summary>
        public ICollection<Func<IEnumerable<EnumLocalizationDefine>>> EnumLocalizationProviders { get; internal set; } = new List<Func<IEnumerable<EnumLocalizationDefine>>>();
        // public Func<IDispatcher, IActiveUnitOfWork, ICapTransaction> wt;

        /// <summary>
        /// <para>可选tag数据提供器</para>
        /// 最外层：key是实体类型全面 + 可选的属性名；
        /// input.EntityType + (input.PropertyName.IsNotNullOrWhiteSpaceBXJG() ? "_" + input.PropertyName : "");
        /// <para>
        /// value是这种类型数据的权限依赖permissionDependency（AnonymousPermissionDependency.Instance AuthenticatePermissionDependency.Instance new SimplePermissionDependency）
        ///  </para>
        /// <para>和多个数据提供来源，每个委托对应一个，多个来源的数据后者覆盖前者，合并
        /// </para>
        /// 
        /// <para>委托中的第1个string是实体类型全名</para>
        /// <para>委托中的第2个string?是可选的属性名</para>
        /// <para>到后期，tab中相同类型的tag都会越来越多，所以需要数量限制，这个由前端和默认值控制，委托中的int就是这个含义</para>
        /// 
        /// 
        /// <para>
        /// 具体的委托实现可以从字典、配置、内存、或任意地方来
        /// tag表去重这种方式可以不单独提供，因为应用服务默认会做这个查询。
        /// </para><para>
        /// 应用服务统一向前端提供数据，它的数据来源就是这里
        /// 你也可以完全忽略对应的应用服务和这里的配置，而是自己实现可选tag列表</para>
        /// 
        /// <para>
        /// 为啥一个数据类型，需要多个委托？
        /// 因为这是公共模块，它可能被同一个项目的多个模块引用，不同模块可能对相同的数据类型提供多个委托
        /// 类似abp菜单、权限提供器，只不过它们使用了接口，而我们是使用委托，避免定义接口了
        /// </para>
        /// 
        /// <para>
        /// 是否需要从tags数据表中获取，最好是用提供器的方式，根据不同数据类型取决定，这样更灵活，
        /// 但这个委托可以进一步封装
        /// </para>
        /// 
        /// </summary>
        public IDictionary<string, SelectableTagProviderEntry> SelectableTagProviders = new Dictionary<string, SelectableTagProviderEntry>();

        public void AddSelectableTagProvider(string entityType,
                                             Func<SelectableTagContext, ValueTask<List<SelectableTagDto>>> provider,
                                             string? propertyName = default, string? propertyDisplayName = default,
                                             IPermissionDependency? permissionDependency = default,
                                             bool loadFromDb = true)
        {
            //防止冲突，无论如何都加个点
            var key = entityType + "."+(propertyName.IsNotNullOrWhiteSpaceBXJG() ?  propertyName :string.Empty);
            if (!SelectableTagProviders.TryGetValue(key, out var lb))
            {
                lb = new SelectableTagProviderEntry(entityType,  propertyName,
                    new List<Func<SelectableTagContext, ValueTask<List<SelectableTagDto>>>>(),
                    permissionDependency ?? AnonymousPermissionDependency.Instance,
                   propertyDisplayName??propertyName,
                    loadFromDb);
                SelectableTagProviders.Add(key, lb);
            }

            lb.Providers.Add(provider);
        }

        public void AddSelectableTagProvider<TEntity>(Func<SelectableTagContext, ValueTask<List<SelectableTagDto>>> provider,
                                                     string? propertyName = default, string? propertyDisplayName = default,
                                                     IPermissionDependency? permissionDependency = default,
                                                     bool loadFromDb = true)
        {
            AddSelectableTagProvider(typeof(TEntity).FullName, provider, propertyName, propertyDisplayName, permissionDependency, loadFromDb);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="EntityType"></param>
        /// <param name="Providers"></param>
        /// <param name="PermissionDependency"></param>
        /// <param name="PropertyName"></param>
        /// <param name="PropertyDisplayName"></param>
        /// <param name="LoadFromDb"></param>
        public record class SelectableTagProviderEntry(
            string EntityType,
                                    string? PropertyName ,
                                    ICollection<Func<SelectableTagContext, ValueTask<List<SelectableTagDto>>>> Providers,
            IPermissionDependency PermissionDependency,
                                    string? PropertyDisplayName = default,
                                    bool LoadFromDb = true);


        public BXJGUtilsModuleConfig()
        {
            //诡异的问题，在这里初始化后，  调用方从容器中获取或注入的单例对象的Enums是一个数组
            //Enums = new List<EnumConfigItem>();

        }
    }
}

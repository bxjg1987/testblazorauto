using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Services;

namespace BXJG.DynamicAssociateEntity
{
    public class DynamicAssociateEntityDefineManager : ISingletonDependency
    {
        //Defines、GroupedDefines 可以定义到模块配置中，目前只是参考abp setting的设计，放manager里的

        /// <summary>
        /// 全局动态关联定义
        /// key：唯一名称；value：定义
        /// 这种结构比List访问更快
        /// </summary>
        public IReadOnlyDictionary<string, DynamicAssociateEntityDefine> Defines { get; protected set; }
        protected readonly IIocManager _iocManager;
        /// <summary>
        /// 分组后的动态关联实体定义 key就是value.GroupName,这么做是为了提高性能
        /// </summary>
        public IReadOnlyDictionary<string, DefineMapGroup> GroupedDefines { get; protected set; }
        protected readonly DynamicAssociateEntityConfiguration dynamicAssociateEntityConfiguration;

        public DynamicAssociateEntityDefineManager(IIocManager iocManager, DynamicAssociateEntityConfiguration dynamicAssociateEntityConfiguration)
        {
            _iocManager = iocManager;
            //_settings = new Dictionary<string, IReadOnlyList<DynamicAssociateEntityDefine>>();
            this.dynamicAssociateEntityConfiguration = dynamicAssociateEntityConfiguration;
        }

        //public IReadOnlyList<DynamicAssociateEntityDefine> Get(string key)
        //{
        //    throw new NotImplementedException();
        // return dynamicAssociateEntityConfiguration.DynamicAssociateEntityDefines[key].AsReadOnly();
        //}

        public void Initialize()
        {
            var ctx = new DynamicAssociateEntityDefineProviderContext();
            var dic = new Dictionary<string, DynamicAssociateEntityDefine>();
            foreach (var providerType in dynamicAssociateEntityConfiguration.DynamicAssociateEntityDefineProviders)
            {
                IEnumerable<DynamicAssociateEntityDefine> defines;
                using (var provider = _iocManager.ResolveAsDisposable<IDynamicAssociateEntityDefineProvider>(providerType))
                {
                    defines = provider.Object.GetDefines(ctx);
                }
                foreach (var item in defines)
                {
                    var p = item;
                    if (p != null)
                    {
                        dic.Add(p.Name, p);
                        p = p.Parent;
                    }
                }
            }
            Defines = new ReadOnlyDictionary<string, DynamicAssociateEntityDefine>(dic);




            var groupContext = new DynamicAssociateEntityDefineGroupProviderContext();
            var groupTemp = new Dictionary<string, DefineMapGroup>();
            /*
             {
                key:'groupName',
                items:[
                    { defineName:'product', 关联粒度：row }
                ]
             }
             */
            var maps = dynamicAssociateEntityConfiguration.DynamicAssociateEntityDefineGroupProvider(groupContext);
            foreach (var item in maps)
            {
                var grp = new DefineMapGroup
                {
                    GroupName = item.Key,
                    Items = item.Value.Select(c => new DefineMapItem
                    {
                        Define = Defines[c.Name],
                        AssociateGranularity = c.AssociateGranularity
                    }).ToList().AsReadOnly()
                };
                groupTemp.Add(grp.GroupName, grp);
            }
            GroupedDefines = new ReadOnlyDictionary<string, DefineMapGroup>(groupTemp);
        }
    }
    public class DefineMapGroup
    {
        public string GroupName { get; set; }

        public IReadOnlyList<DefineMapItem> Items { get; set; }

        //这里将来可能增加更多属性
    }
    public class DefineMapItem
    {
        public DynamicAssociateEntityDefine Define { get; set; }
        public AssociateGranularity AssociateGranularity { get; set; }
    }
}

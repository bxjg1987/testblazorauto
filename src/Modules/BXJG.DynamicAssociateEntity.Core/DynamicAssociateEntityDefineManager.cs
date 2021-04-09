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
            #region 将所有模块中的数据定义注册到全局容器中，也会处理父子关系
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
                    while (true)
                    {
                        dic.Add(p.Name, p);
                        if (p.Child != null)
                        {
                            p.Child.Parent = p;
                            p = p.Child;
                            //p.Name = p.Parent?.Name + p.Name;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            Defines = new ReadOnlyDictionary<string, DynamicAssociateEntityDefine>(dic);
            #endregion

            #region 目标数据与希望关联的数据做映射
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
                //var items = item.Value.Select(c => new DefineMapItem
                //{
                //    Define = Defines[c.Name],
                //    AssociateGranularity = c.AssociateGranularity,
                //    Required = c.Required
                //}).ToList();

                #region 当用户只配置了级联中的子节点时，自动递归将其父节点加入到映射中
                //比如用户只配置了关联到订单明细，则自动将关联订单的映射添加进来
                var items = item.Value.ToList();
                foreach (var item1 in item.Value)
                {
                    var zd = Defines[item1.Name];
                    DynamicAssociateEntityDefine p = zd.Parent;
                    while (p != null)
                    {
                        if (!items.Any(c => c.Name == p.Name))
                        {
                            var idx = items.IndexOf(item1);

                            items.Insert(idx, new AssociateMapItem { AssociateGranularity = item1.AssociateGranularity, Name = p.Name, Required = item1.Required });
                        }
                        p = p.Parent;
                    }
                }
                #endregion

                var grp = new DefineMapGroup
                {
                    GroupName = item.Key,
                    Items = items.Select(c => new DefineMapItem
                    {
                        Define = Defines[c.Name],
                        AssociateGranularity = c.AssociateGranularity,
                        Required = c.Required
                    }).ToList().AsReadOnly()
                };
                groupTemp.Add(grp.GroupName, grp);
            }
            GroupedDefines = new ReadOnlyDictionary<string, DefineMapGroup>(groupTemp);
            #endregion
        }
    }
    public class DefineMapGroup
    {
        public string GroupName { get; set; }
        /// <summary>
        /// 映射的所有节点
        /// </summary>
        public IReadOnlyList<DefineMapItem> Items { get; set; }
        /// <summary>
        /// 映射所有节点中的顶级节点
        /// </summary>
        public IReadOnlyList<DefineMapItem> TopItems => Items.Where(c => c.Define.Parent == null).ToList().AsReadOnly();
        //这里将来可能增加更多属性
    }
    public class DefineMapItem
    {
        public DynamicAssociateEntityDefine Define { get; set; }
        public AssociateGranularity AssociateGranularity { get; set; }
        public bool Required { get; set; }
    }
}

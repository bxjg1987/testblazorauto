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

        public T GetService<T>(string name)
        {
            return (T)_iocManager.Resolve(Defines[name].ServiceType);
        }

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
                    items = items.Select(c => new DefineMapItem
                    {
                        Define = Defines[c.Name],
                        AssociateGranularity = c.AssociateGranularity,
                        Required = c.Required
                    }).ToList().AsReadOnly()
                };
                grp.Init();
                groupTemp.Add(grp.GroupName, grp);
            }

            GroupedDefines = new ReadOnlyDictionary<string, DefineMapGroup>(groupTemp);
            #endregion
        }
    }
    public class DefineMapGroup
    {
        //先调用items进行配置，配置完成后需要调用Init() 

        /// <summary>
        /// 分组名，比如工单需要动态关联到入库单明细、租赁订单明细，则这里的组名为workOrder（工单）
        /// </summary>
        public string GroupName { get; internal set; }

        internal IList<DefineMapItem> items;

        internal void Init()
        {
            #region 建立父子关系
            var listTemp = items.ToList();
            listTemp.ForEach(c => {
                c.Parent = items.SingleOrDefault(d => c.ParentName == d.Name);
                c.Child = items.SingleOrDefault(d => c.Name == d.ChildName);
            });
            #endregion

            Items = listTemp.AsReadOnly();
            TopItems = items.Where(c => c.Define.Parent == null).ToList().AsReadOnly();
            LeafItems = items.Where(c => c.Define.Child == null || !Items.Any(d => d.Define.Name == c.Define.Child.Name)).ToList().AsReadOnly();

            #region 屏蔽以后的递归操作
            var list = new List<IReadOnlyList<DefineMapItem>>();
            foreach (var item in TopItems)
            {
                var list2 = new List<DefineMapItem>();

                var curr = item;
                while (curr != null)
                {
                    list2.Add(curr);
                    curr = curr.Child;
                }
                list.Add(list2.AsReadOnly());
            }
            TopFlatItems = list.AsReadOnly();
            #endregion
        }

        /// <summary>
        /// 映射的所有节点，扁平化结构
        /// 比如工单关联到订单明细时，此属性种既包含订单、也包含订单明细的映射定义
        /// </summary>
        public IReadOnlyList<DefineMapItem> Items { get; private set; }
        /// <summary>
        /// 映射所有节点中的顶级节点，目前是每次计算，后期优化将其存储下来
        /// </summary>
        public IReadOnlyList<DefineMapItem> TopItems { get; private set; }
        /// <summary>
        /// 获取所有顶级节点，扁平化，避免递归，目前是每次计算，后续直接存储下来<br />
        /// [ [equipment],[ order,orderItem ] ] 
        /// </summary>
        public IReadOnlyList<IReadOnlyList<DefineMapItem>> TopFlatItems { get; private set; }
        /// <summary>
        /// 映射所有节点中的叶节点，目前是每次计算，后期优化将其存储下来
        /// </summary>
        public IReadOnlyList<DefineMapItem> LeafItems { get; private set; }

        //这里将来可能增加更多属性
    }
    /// <summary>
    /// 关联关系映射<br />
    /// 比如DefineMapItem的实例1可以表示工单与入库单明细的关联，实例2可以表示工单与物流单关联
    /// </summary>
    public class DefineMapItem
    {
        /// <summary>
        /// 父节点名称,级联关联时，此关联的父节点<br />
        /// 比如工单关联到订单明细时，ParentName为order(订单)，因为订单是订单明细的父级
        /// </summary>
        public string ParentName => Define.ParentName;
        /// <summary>
        /// 节点名称<br />
        /// 比如工单关联到订单明细时，Name为orderItem(订单明细)，关联到物流单时Name为logisticsOrder(物流单)
        /// </summary>
        public string Name => Define.Name;
        /// <summary>
        /// 父节点名称,级联关联时，此关联的父节点<br />
        /// 比如工单关联到订单明细时，Parent为order(订单)，因为订单是订单明细的父级
        /// </summary>
        public DefineMapItem Parent { get; internal set; }
        /// <summary>
        /// 子节点名称,级联关联时，此关联的子节点<br />
        /// 比如工单关联订单明细时，当前节点若是订单，则ChildName为orderItem（订单明细），因为订单是订单明细的父级
        /// </summary>
        public string ChildName => Define.ChildName;
        /// <summary>
        /// 子节点名称,级联关联时，此关联的子节点<br />
        /// 比如工单关联订单明细时，当前节点若是订单，则Child为订单明细节点，因为订单是订单明细的父级
        /// </summary>
        public DefineMapItem Child { get; internal set; }
        /// <summary>
        /// 数据定义
        /// </summary>
        public DynamicAssociateEntityDefine Define { get; internal set; }
        /// <summary>
        /// 关联粒度，比如工单是所有行都关联到物流单？还是没个工单都可能关联到不同的信息
        /// </summary>
        public AssociateGranularity AssociateGranularity { get; internal set; }
        /// <summary>
        /// 此关联是否是必须的
        /// </summary>
        public bool Required { get; internal set; }
    }
}

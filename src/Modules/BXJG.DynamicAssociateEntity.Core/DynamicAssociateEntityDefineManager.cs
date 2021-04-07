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
        /// </summary>
        public IReadOnlyDictionary<string, DynamicAssociateEntityDefine> Defines { get; protected set; }
        protected readonly IIocManager _iocManager;
        /// <summary>
        /// 分组后的动态关联实体定义
        /// key:组名，如workOrder；value：key：数据唯一名，如：product，value：定义
        /// </summary>
        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, DynamicAssociateEntityDefine>> GroupedDefines { get; protected set; }
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

            var groupContext = new DynamicAssociateEntityDefineGroupProviderContext(Defines);
            var groupTemp = new Dictionary<string, IReadOnlyDictionary<string, DynamicAssociateEntityDefine>>();
            var maps = dynamicAssociateEntityConfiguration.DynamicAssociateEntityDefineGroupProvider(groupContext);
            foreach (var item in maps)
            {
                var list = Defines.Where(c => item.Value.Contains(c.Key)).ToDictionary(c => c.Key, c => c.Value);
                groupTemp.Add(item.Key, new ReadOnlyDictionary<string, DynamicAssociateEntityDefine>(list));
            }
            GroupedDefines = new ReadOnlyDictionary<string, IReadOnlyDictionary<string, DynamicAssociateEntityDefine>>(groupTemp);
        }
    }
}

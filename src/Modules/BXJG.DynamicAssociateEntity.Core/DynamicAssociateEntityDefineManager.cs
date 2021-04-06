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
        public IEnumerable<DynamicAssociateEntityDefine> Defines { get; protected set; }
        protected readonly IIocManager _iocManager;
        public IReadOnlyDictionary<string, IReadOnlyList<DynamicAssociateEntityDefine>> GroupedDefines { get; protected set; }
        protected readonly DynamicAssociateEntityConfiguration dynamicAssociateEntityConfiguration;

        public DynamicAssociateEntityDefineManager(IIocManager iocManager, DynamicAssociateEntityConfiguration dynamicAssociateEntityConfiguration)
        {
            _iocManager = iocManager;
            //_settings = new Dictionary<string, IReadOnlyList<DynamicAssociateEntityDefine>>();
            this.dynamicAssociateEntityConfiguration = dynamicAssociateEntityConfiguration;
        }

        public IReadOnlyList<DynamicAssociateEntityDefine> Get(string key)
        {
            throw new NotImplementedException();
            // return dynamicAssociateEntityConfiguration.DynamicAssociateEntityDefines[key].AsReadOnly();
        }


        public void Initialize()
        {
            Defines = dynamicAssociateEntityConfiguration.DynamicAssociateEntityDefineProvider();
            var context = new DynamicAssociateEntityDefineGroupProviderContext(Defines);
            IDictionary<string, IReadOnlyList<DynamicAssociateEntityDefine>> dic = new Dictionary<string, IReadOnlyList<DynamicAssociateEntityDefine>>();
            foreach (var providerType in dynamicAssociateEntityConfiguration.DynamicAssociateEntityDefineGroupProviders)
            {
                using (var provider = CreateProvider(providerType))
                {
                    foreach (var settings in provider.Object.GetDefines(context))
                    {
                        dic.Add(settings.Key, settings.Value.AsReadOnly());
                    }
                }
            }
            GroupedDefines = new ReadOnlyDictionary<string, IReadOnlyList<DynamicAssociateEntityDefine>>(dic);
        }

        private IDisposableDependencyObjectWrapper<IDynamicAssociateEntityDefineGroupProvider> CreateProvider(Type providerType)
        {
            return _iocManager.ResolveAsDisposable<IDynamicAssociateEntityDefineGroupProvider>(providerType);
        }
    }
}

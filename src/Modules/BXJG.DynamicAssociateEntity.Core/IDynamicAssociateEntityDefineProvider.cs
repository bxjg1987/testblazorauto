using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.DynamicAssociateEntity
{
    //public interface IDynamicAssociateEntityDefineProvider
    //{
    //    IEnumerable<DynamicAssociateEntityDefine> GetDefines(DynamicAssociateEntityDefineInitContext context);
    //}

    public interface IDynamicAssociateEntityDefineGroupProvider: ITransientDependency
    {
        Dictionary<string, List<DynamicAssociateEntityDefine>> GetDefines(DynamicAssociateEntityDefineGroupProviderContext context);
    }

    ///// <summary>
    ///// 目前没啥用，预留的
    ///// </summary>
    //public class DynamicAssociateEntityDefineInitContext
    //{

    //}

    public class DynamicAssociateEntityDefineGroupProviderContext
    {
        public DynamicAssociateEntityDefineGroupProviderContext(IEnumerable<DynamicAssociateEntityDefine> defines)
        {
            Defines = defines;
        }

        public IEnumerable<DynamicAssociateEntityDefine> Defines { get; }
    }
}

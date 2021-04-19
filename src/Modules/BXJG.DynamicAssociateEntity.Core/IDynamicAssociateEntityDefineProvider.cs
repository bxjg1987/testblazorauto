using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.DynamicAssociateEntity
{
    public interface IDynamicAssociateEntityDefineProvider : ITransientDependency
    {
        IEnumerable<DynamicAssociateEntityDefine> GetDefines(DynamicAssociateEntityDefineProviderContext context);
    }

    //public interface IDynamicAssociateEntityDefineGroupProvider: ITransientDependency
    //{
    //    Dictionary<string, List<DynamicAssociateEntityDefine>> GetDefines(DynamicAssociateEntityDefineGroupProviderContext context);
    //}

    /// <summary>
    /// 目前没啥用，预留的
    /// </summary>
    public class DynamicAssociateEntityDefineProviderContext
    {

    }

    public class DynamicAssociateEntityDefineGroupProviderContext
    {
        public DynamicAssociateEntityDefineGroupProviderContext(/*IReadOnlyDictionary<string, DynamicAssociateEntityDefine> defines*/)
        {
            //Defines = defines;
        }

        //public IReadOnlyDictionary<string, DynamicAssociateEntityDefine> Defines { get; }
    }
}

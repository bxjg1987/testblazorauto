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
        Dictionary<string, List<DynamicAssociateEntityDefine>> GetDefines(DynamicAssociateEntityDefineInitContext context);
    }

    /// <summary>
    /// 目前没啥用，预留的
    /// </summary>
    public class DynamicAssociateEntityDefineInitContext
    {

    }
}

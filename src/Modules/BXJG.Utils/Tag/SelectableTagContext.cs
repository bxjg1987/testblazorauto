using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Tag
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="EntityType"></param>
    /// <param name="PropertyName"></param>
    /// <param name="Top"></param>
    /// <param name="ScopedIocResolver"></param>
    public record class SelectableTagContext(IScopedIocResolver ScopedIocResolver,string EntityType, string? PropertyName=default,int Top=20);
}

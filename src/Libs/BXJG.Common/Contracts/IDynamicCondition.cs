using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common.Contracts
{
    /// <summary>
    /// 包含动态条件的输入模型应实现此接口
    /// </summary>
    public interface IDynamicCondition
    {
        IEnumerable<ConditionFieldDefine> Conditions { get; set; }
    }
}

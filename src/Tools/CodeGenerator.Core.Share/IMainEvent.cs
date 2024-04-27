using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Core.Share
{
    /*
     * 
     * 多个主流程已经是异步的了，IMainEvent的多个实现就不要异步了，这样事情更简单
     * 可以定义步骤的返回bool来决定是否中断流程，true继续 false中断流程
     * 
     * 
     */

    /// <summary>
    /// 主流程回调
    /// </summary>
    public interface IMainEvent
    {
        ValueTask<bool> Begin(MainFlowContext context);
    }
}

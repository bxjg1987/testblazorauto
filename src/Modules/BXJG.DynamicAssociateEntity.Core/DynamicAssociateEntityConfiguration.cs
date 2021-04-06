using Abp.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.DynamicAssociateEntity
{
    /// <summary>
    /// 动态关联实体配置类<br />
    /// 按abp规范它是单例的，你需要是可以使用ioc注入
    /// </summary>
    public class DynamicAssociateEntityConfiguration
    {
        //严格来说DynamicAssociateEntityDefines.Value只能在初始化阶段可写，后续只读，这里偷懒了。所以abp类似settings系统才使用了单独的provider模式

        //配置对象应该算是底层的，针对动态关联实体定义应该提供专门的DynamicAssociateEntityDefineManager，它的相关操作本质上还是操作DynamicAssociateEntityDefines，
        //只不过它更贴切，严格来说注册的操作也应该放DynamicAssociateEntityDefineManager，所以类似abp settings系统就是这么多的

        //abp settings系统数据最终是存储在manager中的，manager被迫注册为单例，配置中值是用来注册provider，个人觉得原始数据存储在配置对象更合理，比较Module类本身就是单例的

        //---------------------------------------------------------------------------

        //只有启动阶段需要配置 动态实体关联定义 后续只会读取，上面分析了 配置中存储原始数据，DynamicAssociateEntityDefineManager负责独去，可以单独搞个类负责在启动阶段
        //配置 动态实体关联定义 
        public ITypeList<IDynamicAssociateEntityDefineProvider> Providers { get;  } = new TypeList<IDynamicAssociateEntityDefineProvider>();

        public ITypeList<IDynamicAssociateEntityDefineProvider2> Providers2 { get; } = new TypeList<IDynamicAssociateEntityDefineProvider2>();
        ///// <summary>
        ///// 动态关联实体定义列表<br />
        ///// 各模块在启动阶段配置此列表<br />
        ///// key：类别(如：工单)，value：可以关联到到哪些目标实体的定义（如：订单及明细、物流单等），此属性初始化阶段可写，其它位置只读<br />
        ///// 注意，注册是相同DynamicAssociateEntityDefine请保持一个实例
        ///// </summary>
        //public readonly Dictionary<string, List<DynamicAssociateEntityDefine>> DynamicAssociateEntityDefines = new Dictionary<string, List<DynamicAssociateEntityDefine>>();
    }
}
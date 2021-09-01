using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo
{
    /// <summary>
    /// 注册具体物品类型定义 过程中的上下文对象
    /// 它在注册流程中传递数据
    /// </summary>
    public class GoodsInfoTypeDefineAddContex
    { 
    
    }
    /// <summary>
    /// 物品模块配置对象
    /// </summary>
    public class GoodsInfoConfiguration
    {
        /// <summary>
        /// 注册具体物品类型的委托集合
        /// 当你实现具体物品类型时应通过此属性实现物品类型注册
        /// 注册成功后此属性将被设置为null
        /// </summary>
        public List<Func<GoodsInfoTypeDefineAddContex,GoodsInfoTypeDefine>> AddGoodsInfoTypes = new List<Func<GoodsInfoTypeDefineAddContex, GoodsInfoTypeDefine>>();
    }
}

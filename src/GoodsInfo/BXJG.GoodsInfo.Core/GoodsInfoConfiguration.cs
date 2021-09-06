using Abp.Configuration.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo
{
    
    /// <summary>
    /// 物品模块配置对象
    /// </summary>
    public class GoodsInfoConfiguration
    {
        //不要定义一个无用的字段，直接在Module.Configuation.Set中存储，用完后再删除掉
        //private readonly IAbpStartupConfiguration abpStartupConfiguration;
        //public GoodsInfoConfiguration(IAbpStartupConfiguration abpStartupConfiguration) {
        //    this.abpStartupConfiguration = abpStartupConfiguration;
        //}
        //public void AddGoodsType(Func<GoodsInfoTypeDefineAddContex, GoodsInfoTypeDefine> func) {
        //    //abpStartupConfiguration.Get<IList<Func<GoodsInfoTypeDefineAddContex, GoodsInfoTypeDefine>>>();
        //    //abpStartupConfiguration.Set()
        //}

        //也可以不定义AddGoodsInfoTypes属性，直接在Module.Configuation.Set中存储，用完后再删除掉
        //此处用委托比Provider接口更简单

        /// <summary>
        /// 注册具体物品类型的委托集合
        /// 当你实现具体物品类型时应通过此属性实现物品类型注册
        /// 注册成功后此属性将被设置为null
        /// </summary>
        public List<Func<GoodsInfoTypeDefineAddContex,GoodsInfoTypeDefine>> GoodsInfoTypeProviders = new List<Func<GoodsInfoTypeDefineAddContex, GoodsInfoTypeDefine>>();
    }
}

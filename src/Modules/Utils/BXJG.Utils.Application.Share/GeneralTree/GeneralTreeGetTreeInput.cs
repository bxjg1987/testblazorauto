using BXJG.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.GeneralTree
{
    /// <summary>
    /// 树形数据的管理页面查询时的请求模型
    /// </summary>
    public class GeneralTreeGetTreeInput:IReset // 与ForSelectInput不同，后者只针对选择，且有Search和Form模式
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsActive { get; set; }
        //不要使用Nullable<TId>类型，因为某些情况此参数可能不允许为null
        //若数据很多 可能需要么次加载一个节点的数据，所以ParentId字段是必要的
        /// <summary>
        /// 架子指定code下所有后代节点下的数据
        /// code可能会变，所以不要在硬编码时使用code
        /// </summary>
        public string? ParentCode { get; set; }
        /// <summary>
        /// 父级节点id，非必填
        /// </summary>
        public long? ParentId { get; set; } //树形父节点本身就可能是空，所以这里就用long?吧
        ///// <summary>
        ///// 是否加载父节点
        ///// </summary>
        //public bool LoadParent { get; set; }
        ///// <summary>
        ///// 若LoadParent为true且ParentId为空，则自动以{id:0,text:L( ParentText) }创建一个虚拟父节点，注意L是处理本地化的方法，因此ParentText值的是本地化的key
        ///// </summary>
        //public string? ParentText { get; set; }

        /// <summary>
        /// 是否仅仅加载子节点，true只加载子节点，false加载所有后台节点
        /// </summary>
        public bool IsOnlyLoadChild { get; set; } = false;

        public virtual void Reset() {
            ParentCode = default;
            ParentId = default;
            IsOnlyLoadChild = false;
            if (this is IHaveKeywords t)
                t.Keywords = default;
        }

        //由于关键字是or查询，若要让子类和父类的关键字条件在一个or链中，会让父类变得复杂，因此决定不在父类中定义关键字
        ///// <summary>
        ///// 关键字，若为空则限制
        ///// </summary>
        //public string? Keywords { get; set; }

        //没必要在这里定义初始值，因为需要处理本地化
        //若想在这里处理需要引入LocationManager，但是这个类是被模型绑定实例化的，因此比较麻烦，不确定abp的模型绑定是否注入本地化相关类，所以不再这个类的内部来做本地化
    }

    /// <summary>
    /// 后台管理数据字典时，获取列表的输入参数
    /// </summary>
    public class DataDictionaryGetTreeInput : GeneralTreeGetTreeInput, IHaveKeywords
    {
        /// <summary>
        /// 是否是系统预定义的
        /// </summary>
        public bool? IsSysDefine { get; set; }
        public string? Keywords { get; set; }
    }
}
using Abp.Dependency;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo
{
    /// <summary>
    /// 物品类型定义管理器（领域服务），
    /// 调用方在模块初始化阶段注册自己扩展的物品类型，在任何地方ioc注入此对象使用，它是单例注册倒ioc容器中的。
    /// </summary>
    public class GoodsInfoTypeDefineManager : DomainServiceBase, IReadOnlyDictionary<string, GoodsInfoTypeDefine>, IReadOnlyList<GoodsInfoTypeDefine>
    {
        Dictionary<string, GoodsInfoTypeDefine> dic;
        List<GoodsInfoTypeDefine> list;

        public GoodsInfoTypeDefineManager(IList<GoodsInfoTypeDefine> list)
        {
            this.dic = new Dictionary<string, GoodsInfoTypeDefine>(StringComparer.OrdinalIgnoreCase);
            foreach (var item in list)
            {
                dic.Add(item.EntityTypeName, item);
            }
            this.list = list.OrderBy(c => c.OrderIndex).ToList();
        }

        public GoodsInfoTypeDefine this[int index] => ((IReadOnlyList<GoodsInfoTypeDefine>)list)[index];

        public GoodsInfoTypeDefine this[string key] => ((IReadOnlyDictionary<string, GoodsInfoTypeDefine>)dic)[key];

        public int Count => ((IReadOnlyCollection<GoodsInfoTypeDefine>)list).Count;

        public IEnumerable<string> Keys => ((IReadOnlyDictionary<string, GoodsInfoTypeDefine>)dic).Keys;

        public IEnumerable<GoodsInfoTypeDefine> Values => ((IReadOnlyDictionary<string, GoodsInfoTypeDefine>)dic).Values;

        public bool ContainsKey(string key)
        {
            return ((IReadOnlyDictionary<string, GoodsInfoTypeDefine>)dic).ContainsKey(key);
        }

        public IEnumerator<GoodsInfoTypeDefine> GetEnumerator()
        {
            return ((IEnumerable<GoodsInfoTypeDefine>)list).GetEnumerator();
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out GoodsInfoTypeDefine value)
        {
            return ((IReadOnlyDictionary<string, GoodsInfoTypeDefine>)dic).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)list).GetEnumerator();
        }

        IEnumerator<KeyValuePair<string, GoodsInfoTypeDefine>> IEnumerable<KeyValuePair<string, GoodsInfoTypeDefine>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, GoodsInfoTypeDefine>>)dic).GetEnumerator();
        }
    }
}

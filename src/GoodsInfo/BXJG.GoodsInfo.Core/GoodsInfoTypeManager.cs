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
    public class GoodsInfoTypeManager : DomainServiceBase, IReadOnlyDictionary<string, GoodsInfoTypeDefine>, IReadOnlyList<GoodsInfoTypeDefine>
    {
        Dictionary<string, GoodsInfoTypeDefine> dic;
        List<GoodsInfoTypeDefine> list;

        public GoodsInfoTypeManager(IList<GoodsInfoTypeDefine> list)
        {
            this.dic = new Dictionary<string, GoodsInfoTypeDefine>();
            foreach (var item in list)
            {
                dic.Add(item.EntityTypeFullName, item);
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

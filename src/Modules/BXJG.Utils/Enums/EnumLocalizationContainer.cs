using Abp;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Localization;
using Abp.Localization.Sources;

using BXJG.Utils.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace BXJG.Utils.Enums
{
    //此对象不算复杂，没必要提供相应Builder对象

    /// <summary>
    /// 枚举作为下拉框数据定义的容器
    /// 本地化枚举列表的应用场景比较少，因此单独定义个容器，而不是直接使用<see cref="BXJGUtilsModuleConfig"/>作为容器
    /// </summary>
    public class EnumLocalizationContainer :IReadOnlyDictionary<string, EnumLocalizationDefine>, IReadOnlyList<EnumLocalizationDefine>
    {
        IReadOnlyList<EnumLocalizationDefine> list;
        IReadOnlyDictionary<string, EnumLocalizationDefine> dic;

        public EnumLocalizationContainer(IEnumerable<EnumLocalizationDefine> items)
        {
            this.list = items.ToList().AsReadOnly();
            var d = new Dictionary<string, EnumLocalizationDefine>(StringComparer.OrdinalIgnoreCase);
            foreach (var item in list)
            {
                d.Add(item.Name, item);
            }
            this.dic = new ReadOnlyDictionary<string, EnumLocalizationDefine>(d);
        }

        public EnumLocalizationDefine this[int index] => ((IReadOnlyList<EnumLocalizationDefine>)list)[index];

        public EnumLocalizationDefine this[string key] => ((IReadOnlyDictionary<string, EnumLocalizationDefine>)dic)[key];

        public int Count => ((IReadOnlyCollection<EnumLocalizationDefine>)list).Count;

        public IEnumerable<string> Keys => ((IReadOnlyDictionary<string, EnumLocalizationDefine>)dic).Keys;

        public IEnumerable<EnumLocalizationDefine> Values => ((IReadOnlyDictionary<string, EnumLocalizationDefine>)dic).Values;

        public bool ContainsKey(string key)
        {
            return ((IReadOnlyDictionary<string, EnumLocalizationDefine>)dic).ContainsKey(key);
        }

        public IEnumerator<EnumLocalizationDefine> GetEnumerator()
        {
            return ((IEnumerable<EnumLocalizationDefine>)list).GetEnumerator();
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out EnumLocalizationDefine value)
        {
            return ((IReadOnlyDictionary<string, EnumLocalizationDefine>)dic).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)list).GetEnumerator();
        }

        IEnumerator<KeyValuePair<string, EnumLocalizationDefine>> IEnumerable<KeyValuePair<string, EnumLocalizationDefine>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, EnumLocalizationDefine>>)dic).GetEnumerator();
        }
    }
}

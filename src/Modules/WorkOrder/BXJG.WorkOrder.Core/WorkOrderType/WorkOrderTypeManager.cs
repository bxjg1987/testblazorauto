using Abp.Dependency;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderType
{
    /// <summary>
    /// 工单类型管理器，单例注册的
    /// </summary>
    public class WorkOrderTypeManager: IReadOnlyDictionary<string, WorkOrderTypeDefine>
    {
        /// <summary>
        /// 获取所有工单类型
        /// </summary>
        private readonly IReadOnlyDictionary<string, WorkOrderTypeDefine> defines;
        public readonly IList<WorkOrderTypeDefine> List;

        public IEnumerable<string> Keys => defines.Keys;

        public IEnumerable<WorkOrderTypeDefine> Values => defines.Values;

        public int Count => defines.Count;

        public WorkOrderTypeDefine this[string key] => defines[key];

        /// <summary>
        /// 实例化工单类型管理器
        /// </summary>
        /// <param name="defines">工单类型列表</param>
        public WorkOrderTypeManager(IList<WorkOrderTypeDefine> defines)
        {
            List = defines;
            var dic = new Dictionary<string, WorkOrderTypeDefine>();
            foreach (var item in defines)
            {
                dic.Add(item.Name, item);
            }
            this.defines = new ReadOnlyDictionary<string, WorkOrderTypeDefine>(dic);
         
        }

        //public void Check(string typeName) {
        //    if (!defines.ContainsKey(typeName))
        //        throw new ApplicationException("无效的工单类型");
        //}

        public bool ContainsKey(string key)
        {
            return defines.ContainsKey(key);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out WorkOrderTypeDefine value)
        {
            return defines.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<string, WorkOrderTypeDefine>> GetEnumerator()
        {
            return defines.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)defines).GetEnumerator();
        }
    }
}

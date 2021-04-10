using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Dynamic;
using Abp.UI;
using Abp.Extensions;
using Abp.Dependency;


namespace BXJG.DynamicAssociateEntity
{
    public static class DynamicAssociateEntityExt
    {
        //一个Dictionary与一个json对象是对应的 
        //SortedDictionary是自动安key排序的

        /// <summary>
        /// 将提交的动态关联值映射为要存储的json格式，通常保存数据且要转换格式时使用<br />
        /// 它与<see cref="CheckMap"/>逻辑一致，但除了检查，还会转换数据格式<br />
        /// </summary>
        /// <param name="defines"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static List<List<KeyValuePair<string, object>>> MapToEntity(this IDictionary<string, object> dto, IReadOnlyList<IReadOnlyList<DefineMapItem>> defines)
        {
            var r = new List<List<KeyValuePair<string, object>>>();
            foreach (var item in defines)
            {
                var list = new List<KeyValuePair<string, object>>();
                for (var i = item.Count - 1; i >= 0; i--)
                {
                    var define = item[i];
                    if (!dto.ContainsKey(define.Name) || dto[define.Name] == null || dto[define.Name].ToString().IsNullOrWhiteSpace())
                    {
                        if (define.Required)
                            throw new ApplicationException("要求必填");
                        else
                            continue;
                    }

                    if (define.Parent != null && (!dto.ContainsKey(define.ParentName) || dto[define.ParentName] == null || dto[define.ParentName].ToString().IsNullOrWhiteSpace()))
                        throw new ApplicationException("递归父级必填");
                    list.Add(new KeyValuePair<string, object>(define.Name,dto[define.Name]));
                }
                list.Reverse();
                r.Add(list);
            }
            return r;
        }
        /// <summary>
        /// 将提交的动态关联值映射为要存储的json格式，通常保存数据且要转换格式时使用<br />
        /// 它与<see cref="CheckMap"/>逻辑一致，但除了检查，还会转换数据格式
        /// </summary>
        /// <param name="defines">dynamicAssociateEntityDefineManager.GroupedDefines[name].LeafItems</param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static string DtoMapToEntityJsonString(this IDictionary<string, object> dto, IReadOnlyList<IReadOnlyList<DefineMapItem>> defines)
        {
            return JsonSerializer.Serialize(dto.MapToEntity(defines));
        }
        ///// <summary>
        ///// 检查动态关联值是否与配置的映射匹配<br />
        ///// 查询数据虽然也可以调用，但不建议调用，有点影响性能<br />
        ///// 它与<see cref="DtoMapToEntity"/>逻辑一致，但这是做判断
        ///// </summary>
        ///// <param name="defines">dynamicAssociateEntityDefineManager.GroupedDefines[name].LeafItems</param>
        ///// <param name="dto"></param>
        //public static void CheckMap(this IDictionary<string, object> dto, IReadOnlyList<IReadOnlyList<DefineMapItem>> defines)
        //{
        //    #region 数据格式
        //    /*
        //     * dto
        //     * [
        //     *      { "a" : 3 },
        //     *      { "b" : 6 }
        //     * ]
        //     * 
        //     * entity
        //     * [
        //     *      { "a" : 3 },
        //     *      { "b" : 6 ,"child":{ "p":734 }}
        //     * ]
        //     * 
        //     * defines 只包含顶级节点，且递归子节点只到允许的节点
        //     * 若叶节点无值的话 但要求必填则报错
        //     * 若叶节点有值的话，递归父节点必须有值
        //     * 所以只能从叶子节点 逐层网上
        //     */
        //    #endregion

        //    foreach (var item in defines)
        //    {
        //        var define = item.Define;
        //        dynamic child = null;
        //        while (define != null)
        //        {
        //            if (!dto.ContainsKey(define.Name) || dto[define.Name] == null || dto[define.Name].ToString().IsNullOrWhiteSpace())
        //            {
        //                //若设了必填，或存在叶节点值时，当前节点必填
        //                if (item.Required || child != null)
        //                    throw new UserFriendlyException($"{define.Name}为必填");
        //                else
        //                    break;
        //            }

        //            dynamic entityItem = new ExpandoObject();
        //            //(entityItem as IDictionary<string, object>)[define.Name] = dto[define.Name];
        //            if (child == null)
        //            {
        //                child = entityItem;
        //            }
        //            else
        //            {
        //                entityItem.Child = child;
        //                child = entityItem;
        //            }
        //            define = define.Parent;
        //        }
        //    }
        //}

        /// <summary>
        /// 转换为动态关联的值
        /// </summary>
        /// <param name="str">[ { a:1,child:{ b:2, child:{ c:3 } } },{ d:1 } ]</param>
        /// <returns></returns>
        public static List<List<KeyValuePair<string, object>>> ToDynamicAssociateData(this string str)
        {
            return JsonSerializer.Deserialize<List<List<KeyValuePair<string, object>>>>(str);
        }
        /// <summary>
        /// 转换为动态关联的值
        /// </summary>
        /// <param name="entity">entity.DynamicAssociateData为[ { a:1,child:{ b:2, child:{ c:3 } } },{ d:1 } ]</param>
        /// <returns></returns>
        public static List<List<KeyValuePair<string, object>>> ToDynamicAssociateData(this IDynamicAssociateEntity entity)
        {
            return entity.DynamicAssociateData.ToDynamicAssociateData();
        }
        ///// <summary>
        ///// 递归动态关联的值的级联子节点
        ///// </summary>
        ///// <param name="dic">{ a:1,child:{ b:2, child:{ c:3 } } }</param>
        ///// <param name="act">递归过程种的回调，参数1：父节点，参数2：子节点</param>
        ///// <param name="parent">方法内部使用，你无需传递此参数</param>
        //public static void DynamicAssociateRecurrenceChild(this IDictionary<string, object> dic,
        //                                                   Action<KeyValuePair<string, object>?, KeyValuePair<string, object>> act,
        //                                                   KeyValuePair<string, object>? parent = default)
        //{
        //    //我们规定的数据结构决定了 可以使用下面的方式来做递归

        //    if (dic.Count == 1)
        //    {
        //        act(parent, dic.Single());
        //    }
        //    else
        //    {
        //        //要求child字符串小写，不做大小写忽略，性能更高
        //        var curr = dic.Where(c => c.Key != "child").Single();
        //        var child = dic["child"] as IDictionary<string, object>;
        //        child.DynamicAssociateRecurrenceChild(act, curr);
        //    }
        //}
    }
}

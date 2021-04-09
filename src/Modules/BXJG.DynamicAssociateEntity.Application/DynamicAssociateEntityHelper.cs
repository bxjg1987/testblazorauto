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
    public class DynamicAssociateEntityHelper
    {
        protected readonly DynamicAssociateEntityDefineManager dynamicAssociateEntityDefineManager;
        protected readonly DefineMapGroup defines;
        protected readonly IIocResolver iocResolver;
        public DynamicAssociateEntityHelper(DynamicAssociateEntityDefineManager dynamicAssociateEntityDefineManager, string groupName, IIocResolver iocResolver)
        {
            this.dynamicAssociateEntityDefineManager = dynamicAssociateEntityDefineManager;
            defines = dynamicAssociateEntityDefineManager.GroupedDefines[groupName];
            this.iocResolver = iocResolver;
        }

        public IList<dynamic> DtoMapToEntity(IDictionary<string, object> dto)
        {
            return DtoMapToEntity(defines.LeafItems, dto);
        }
        public string DtoMapToEntityJsonString(IDictionary<string, object> dto)
        {
            return DtoMapToEntityJsonString(defines.LeafItems, dto);
        }
        /// <summary>
        /// 将用户提交的动态关联外键的数据转换映射到实体的对应属性上，最终以json格式保存，且保留级联结构
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="entity"></param>
        public void DtoMapToEntity(IDynamicAssociateEditDto dto, IDynamicAssociateEntity entity)
        {
            entity.DynamicAssociateData = DtoMapToEntityJsonString(dto.DynamicAssociateData);
        }

        public IDynamicAssociateEntityService GetService(string name) {
            return iocResolver.Resolve(dynamicAssociateEntityDefineManager.Defines[name].ServiceType) as IDynamicAssociateEntityService;
        }

        #region 静态方法
        /// <summary>
        /// 将提交的动态关联值映射为要存储的json格式，通常保存数据且要转换格式时使用<br />
        /// 它与<see cref="CheckMap"/>逻辑一致，但除了检查，还会转换数据格式
        /// </summary>
        /// <param name="defines">dynamicAssociateEntityDefineManager.GroupedDefines[name].LeafItems</param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static IList<dynamic> DtoMapToEntity(IReadOnlyList<DefineMapItem> defines, IDictionary<string, object> dto)
        {
            #region 数据格式
            /*
             * dto
             * [
             *      { "a" : 3 },
             *      { "b" : 6 }
             * ]
             * 
             * entity
             * [
             *      { "a" : 3 },
             *      { "b" : 6 ,"child":{ "p":734 }}
             * ]
             * 
             * defines 只包含顶级节点，且递归子节点只到允许的节点
             * 若叶节点无值的话 但要求必填则报错
             * 若叶节点有值的话，递归父节点必须有值
             * 所以只能从叶子节点 逐层网上
             */
            #endregion

            var targets = new List<dynamic>();//准备要返回的数据格式
            foreach (var item in defines)
            {
                var define = item.Define;
                dynamic child = null;
                while (define != null)
                {
                    if (!dto.ContainsKey(define.Name) || dto[define.Name] == null || dto[define.Name].ToString().IsNullOrWhiteSpace())
                    {
                        //若设了必填，或存在叶节点值时，当前节点必填
                        if (item.Required || child != null)
                            throw new UserFriendlyException($"{define.Name}为必填");
                        else
                            break;
                    }

                    dynamic entityItem = new ExpandoObject();
                    (entityItem as IDictionary<string, object>)[define.Name] = dto[define.Name];
                    if (child == null)
                    {
                        child = entityItem;
                    }
                    else
                    {
                        entityItem.Child = child;
                        child = entityItem;
                    }
                    define = define.Parent;
                }
                targets.Add(child);
            }
            return targets;
        }
        /// <summary>
        /// 将提交的动态关联值映射为要存储的json格式，通常保存数据且要转换格式时使用<br />
        /// 它与<see cref="CheckMap"/>逻辑一致，但除了检查，还会转换数据格式
        /// </summary>
        /// <param name="defines">dynamicAssociateEntityDefineManager.GroupedDefines[name].LeafItems</param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static string DtoMapToEntityJsonString(IReadOnlyList<DefineMapItem> defines, IDictionary<string, object> dto)
        {
            return JsonSerializer.Serialize(DtoMapToEntity(defines, dto));
        }
        /// <summary>
        /// 检查动态关联值是否与配置的映射匹配<br />
        /// 查询数据虽然也可以调用，但不建议调用，有点影响性能<br />
        /// 它与<see cref="DtoMapToEntity"/>逻辑一致，但这是做判断
        /// </summary>
        /// <param name="defines">dynamicAssociateEntityDefineManager.GroupedDefines[name].LeafItems</param>
        /// <param name="dto"></param>
        public static void CheckMap(IReadOnlyList<DefineMapItem> defines, IDictionary<string, object> dto)
        {
            #region 数据格式
            /*
             * dto
             * [
             *      { "a" : 3 },
             *      { "b" : 6 }
             * ]
             * 
             * entity
             * [
             *      { "a" : 3 },
             *      { "b" : 6 ,"child":{ "p":734 }}
             * ]
             * 
             * defines 只包含顶级节点，且递归子节点只到允许的节点
             * 若叶节点无值的话 但要求必填则报错
             * 若叶节点有值的话，递归父节点必须有值
             * 所以只能从叶子节点 逐层网上
             */
            #endregion

            foreach (var item in defines)
            {
                var define = item.Define;
                dynamic child = null;
                while (define != null)
                {
                    if (!dto.ContainsKey(define.Name) || dto[define.Name] == null || dto[define.Name].ToString().IsNullOrWhiteSpace())
                    {
                        //若设了必填，或存在叶节点值时，当前节点必填
                        if (item.Required || child != null)
                            throw new UserFriendlyException($"{define.Name}为必填");
                        else
                            break;
                    }

                    dynamic entityItem = new ExpandoObject();
                    //(entityItem as IDictionary<string, object>)[define.Name] = dto[define.Name];
                    if (child == null)
                    {
                        child = entityItem;
                    }
                    else
                    {
                        entityItem.Child = child;
                        child = entityItem;
                    }
                    define = define.Parent;
                }
            }
        }
        #endregion
    }
}
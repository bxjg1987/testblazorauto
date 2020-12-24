using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.DynamicEntityProperties;
using Abp.Extensions;
using Abp.UI.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Abp.Dependency;

namespace BXJG.Utils.DynamicProperty
{
    /// <summary>
    /// 设计时考虑商品的sku动态属性设计场景<br/>
    /// 在模块<see cref="BXJGUtilsModule.Initialize"/>中注册到ioc
    /// </summary>
    /// <typeparam name="TEntity">只是用来获取字符串</typeparam>
    public class DynamicPropertyAppService<TEntity> : ITransientDependency
    {
        private readonly Abp.DynamicEntityProperties.DynamicPropertyManager propertyManager;
        private readonly DynamicPropertyValueManager valueManager;
        private readonly DynamicEntityPropertyStore dynamicEntityPropertyStore;
        private readonly string entityType;
        private readonly IUnitOfWorkManager unitOfWorkManager;

        public DynamicPropertyAppService(DynamicPropertyManager propertyManager, DynamicPropertyValueManager valueManager, DynamicEntityPropertyStore dynamicEntityPropertyStore, IUnitOfWorkManager unitOfWorkManager)
        {
            this.propertyManager = propertyManager;
            this.valueManager = valueManager;
            this.dynamicEntityPropertyStore = dynamicEntityPropertyStore;
            entityType = typeof(TEntity).FullName;
            this.unitOfWorkManager = unitOfWorkManager;
        }
        [UnitOfWork]
        public async Task<List<Abp.DynamicEntityProperties.DynamicEntityProperty>> SetDynamicPropertyAsync(IEnumerable<DynamicPropertyEditDto> input, object id)
        {
            /*
             * 经过测试，abp提供的动态属性相关领域服务和存储已经自动处理了租户id
             */

            #region 删除原来的动态属性相关信息
            //动态实体属性和动态属性值有级联删除
            var sss = await dynamicEntityPropertyStore.GetAllAsync(entityType + id);
            foreach (var item in sss)
            {
                await propertyManager.DeleteAsync(item.DynamicPropertyId);
                //await valueManager.GetAllValuesOfDynamicPropertyAsync(item.DynamicPropertyId);
            }
            #endregion

            var inputType = InputTypeBase.GetName<ComboboxInputType>();
            #region 添加动态属性相关信息
            foreach (var c in input)
            {
                //若是不符要求的输入则跳过
                if (c.DisplayName.IsNullOrWhiteSpace())
                    continue;
                if (c.PropertyName.IsNullOrWhiteSpace())
                    continue;
                if (c.InputType.IsNullOrWhiteSpace())
                    continue;
                if (c.InputType.Equals(inputType, StringComparison.OrdinalIgnoreCase) && c.PropertyValues.IsNullOrWhiteSpace())
                    continue;

                //动态属性
                var dp = new Abp.DynamicEntityProperties.DynamicProperty
                {
                    DisplayName = c.DisplayName,
                    InputType = c.InputType.ToUpper(),
                    //TenantId = tenantId, 不提供 看看abp自己会处理不
                    PropertyName = c.PropertyName + id
                };

                await propertyManager.AddAsync(dp);
                await unitOfWorkManager.Current.SaveChangesAsync();
                if (c.InputType.Equals(inputType, StringComparison.OrdinalIgnoreCase))
                {
                    var p = c.PropertyValues.Replace('，', ',').Split(',');//反下，否则ef保存是反着的
                    foreach (var item in p)
                    {
                        await valueManager.AddAsync(new DynamicPropertyValue(dp, item, default));
                    }
                }

                //限制某类别下面的商品可选动态属性
                await dynamicEntityPropertyStore.AddAsync(new DynamicEntityProperty
                {
                    DynamicPropertyId = dp.Id,
                    EntityFullName = entityType + id,
                    //TenantId = tenantId 不提供 看看abp自己会处理不
                });
            }
            await unitOfWorkManager.Current.SaveChangesAsync();
            return await GetDynamicPropertyAsync(id);
            #endregion
        }

        public async Task<List<Abp.DynamicEntityProperties.DynamicEntityProperty>> GetDynamicPropertyAsync(object id)
        {
            var ls = await dynamicEntityPropertyStore.GetAllAsync(entityType + id);
            //var ids = ls.Select(c => c.DynamicPropertyId).Distinct();
            //var list = = new List<Abp.DynamicEntityProperties.DynamicEntityProperty>();
            foreach (var item in ls)
            {
                //var t = new DynamicPropertyEditDto
                //{
                //    DisplayName = item.DynamicProperty.DisplayName,
                //    InputType = item.DynamicProperty.InputType,
                //    PropertyName = item.DynamicProperty.PropertyName.TrimStart(input.Id.ToString().ToArray())//abp默认的动态属性是全局唯一的，我们这里为了方便用户为每个类别建立自己的动态属性约束，加上id
                //};
                await valueManager.GetAllValuesOfDynamicPropertyAsync(item.DynamicPropertyId);
                //m.DynamicProperty.Add(t);
            }
            return ls;
        }
    }

    //由于Util没有分层，所以这个类没有放Application层
    public static class DynamicEntityPropertyExtensions
    {
        public static List<DynamicPropertyDto> ToDto(this List<Abp.DynamicEntityProperties.DynamicEntityProperty> dynamicEntityProperties)
        {
            return dynamicEntityProperties.Select(c => new DynamicPropertyDto
            {
                Id = c.DynamicPropertyId,
                DisplayName = c.DynamicProperty.DisplayName,
                InputType = c.DynamicProperty.InputType,
                PropertyName = Regex.Replace(c.DynamicProperty.PropertyName, @"\d+", ""), //c.DynamicProperty.PropertyName.TrimEnd(id.ToString().ToArray()),
                DynamicPropertyValues = c.DynamicProperty.DynamicPropertyValues?.ToDictionary(cc => cc.Id, cc => cc.Value)
            }).ToList();
        }
    }
}

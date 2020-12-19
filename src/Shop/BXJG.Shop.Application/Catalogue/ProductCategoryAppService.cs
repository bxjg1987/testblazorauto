using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using BXJG.Shop.Authorization;
using BXJG.Utils.File;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.DynamicEntityProperties.Extensions;
using Abp.DynamicEntityProperties;
using Abp.UI.Inputs;
using Abp.Application.Services.Dto;
using BXJG.Utils.DynamicProperty;
using Abp.Domain.Uow;

namespace BXJG.Shop.Catalogue
{
    public class ProductCategoryAppService : GeneralTreeAppServiceBase<ProductCategoryDto,
                                                                           ProductCategoryEditDto,
                                                                           ProductCategoryGetAllInput,
                                                                           ProductCategoryGetForSelectInput,
                                                                           ProductCategoryTreeNodeDto,
                                                                           ProductCategoryGetForSelectInput,
                                                                           ProductCategoryCombboxDto,
                                                                           GeneralTreeNodeMoveInput,
                                                                           ProductCategoryEntity,
                                                                           ProductCategoryManager>, IProductCategoryAppService
    {
        private readonly DynamicPropertyManager propertyManager;
        private readonly DynamicPropertyValueManager valueManager;
        private readonly DynamicEntityPropertyStore dynamicEntityPropertyManager;

        private readonly TempFileManager tempFileManager;
        public ProductCategoryAppService(IRepository<ProductCategoryEntity, long> ownRepository,
                                         TempFileManager tempFileManager,
                                         ProductCategoryManager organizationUnitManager,
                                         DynamicPropertyManager propertyManager,
                                         DynamicEntityPropertyStore dynamicEntityPropertyManager,
                                         DynamicPropertyValueManager valueManager) : base(ownRepository,
                                                                                          organizationUnitManager,
                                                                                          PermissionNames.ProductCategoryCreate,
                                                                                          PermissionNames.ProductCategoryUpdate,
                                                                                          PermissionNames.ProductCategoryDelete,
                                                                                          PermissionNames.ProductCategory)
        {
            this.tempFileManager = tempFileManager;
            this.propertyManager = propertyManager;
            this.valueManager = valueManager;
            this.dynamicEntityPropertyManager = dynamicEntityPropertyManager;
        }

        public override async Task<ProductCategoryDto> CreateAsync(ProductCategoryEditDto input)
        {
            if (!input.Icon.IsNullOrWhiteSpace())
                input.Icon = (await this.tempFileManager.MoveAsync(input.Icon)).Single().FileRelativePath;
            if (!input.Image1.IsNullOrWhiteSpace())
                input.Image1 = (await this.tempFileManager.MoveAsync(input.Image1)).Single().FileRelativePath;
            if (!input.Image2.IsNullOrWhiteSpace())
                input.Image2 = (await this.tempFileManager.MoveAsync(input.Image2)).Single().FileRelativePath;

            //先保持，后面的动态属性需要引用实体id
            var m = await base.CreateAsync(input);

            //动态属性处理
            foreach (var c in input.DynamicProperty)
            {
                //若是不符要求的输入则跳过
                if (c.DisplayName.IsNullOrWhiteSpace())
                    continue;
                if (c.PropertyName.IsNullOrWhiteSpace())
                    continue;
                if (c.InputType.IsNullOrWhiteSpace())
                    continue;
                if (c.InputType.Equals("COMBOBOX", StringComparison.OrdinalIgnoreCase) && c.DynamicPropertyValues.IsNullOrWhiteSpace())
                    continue;

                //动态属性
                //SingleLineStringInputType
                var dp = new DynamicProperty
                {
                    DisplayName = c.DisplayName,
                    InputType = c.InputType.ToUpper(),
                    TenantId = base.AbpSession.TenantId,
                    PropertyName =m.Id+ c.PropertyName //abp默认的动态属性是全局唯一的，我们这里为了方便用户为每个类别建立自己的动态属性约束，加上id
                };

                //if (c.InputType == "SingleLineStringInputType")
                //    dp.InputType = InputTypeBase.GetName<SingleLineStringInputType>();

                await propertyManager.AddAsync(dp);
                await base.CurrentUnitOfWork.SaveChangesAsync();
                if (dp.InputType.Equals("COMBOBOX", StringComparison.OrdinalIgnoreCase))
                {
                    var p = c.DynamicPropertyValues.Split(',').Reverse();//反下，否则ef保存是反着的
                    foreach (var item in p)
                    {
                        await valueManager.AddAsync(new DynamicPropertyValue(dp, item, base.AbpSession.TenantId));
                    }
                }

                //限制某类别下面的商品可选动态属性
                await dynamicEntityPropertyManager.AddAsync(new DynamicEntityProperty
                {
                    DynamicPropertyId = dp.Id,
                    EntityFullName = typeof(SkuEntity).FullName + m.Id,
                    TenantId = AbpSession.TenantId
                });
            }
            return m;
        }

        [UnitOfWork]
        public override async Task<ProductCategoryDto> UpdateAsync(ProductCategoryEditDto input)
        {
            if (!input.Icon.IsNullOrWhiteSpace())
                input.Icon = (await this.tempFileManager.MoveAsync(input.Icon)).Single().FileRelativePath;
            if (!input.Image1.IsNullOrWhiteSpace())
                input.Image1 = (await this.tempFileManager.MoveAsync(input.Image1)).Single().FileRelativePath;
            if (!input.Image2.IsNullOrWhiteSpace())
                input.Image2 = (await this.tempFileManager.MoveAsync(input.Image2)).Single().FileRelativePath;

            var m = await base.UpdateAsync(input);
            //base.UnitOfWorkManager
            #region 动态属性处理

            #region 删除原来的动态属性相关信息
            //动态实体属性和动态属性值有级联删除
            var sss = await dynamicEntityPropertyManager.GetAllAsync(typeof(SkuEntity).FullName + m.Id);
            foreach (var item in sss)
            {
                await propertyManager.DeleteAsync(item.DynamicPropertyId);
            }
            #endregion

            #region 添加动态属性相关信息
            foreach (var c in input.DynamicProperty)
            {
                //若是不符要求的输入则跳过
                if (c.DisplayName.IsNullOrWhiteSpace())
                    continue;
                if (c.PropertyName.IsNullOrWhiteSpace())
                    continue;
                if (c.InputType.IsNullOrWhiteSpace())
                    continue;
                if (c.InputType.Equals("COMBOBOX", StringComparison.OrdinalIgnoreCase) && c.DynamicPropertyValues.IsNullOrWhiteSpace())
                    continue;

                //动态属性
                //SingleLineStringInputType
                var dp = new DynamicProperty
                {
                    DisplayName = c.DisplayName,
                    InputType = c.InputType.ToUpper(),
                    TenantId = base.AbpSession.TenantId,
                    PropertyName = m.Id+ c.PropertyName
                };

                //if (c.InputType == "SingleLineStringInputType")
                //    dp.InputType = InputTypeBase.GetName<SingleLineStringInputType>();

                await propertyManager.AddAsync(dp);
                await base.CurrentUnitOfWork.SaveChangesAsync();
                if (c.InputType.Equals("COMBOBOX", StringComparison.OrdinalIgnoreCase))
                {
                    var p = c.DynamicPropertyValues.Split(',').Reverse();//反下，否则ef保存是反着的
                    foreach (var item in p)
                    {
                        await valueManager.AddAsync(new DynamicPropertyValue(dp, item, base.AbpSession.TenantId));
                    }
                }

                //限制某类别下面的商品可选动态属性
                await dynamicEntityPropertyManager.AddAsync(new DynamicEntityProperty
                {
                    DynamicPropertyId = dp.Id,
                    EntityFullName = typeof(SkuEntity).FullName + m.Id,
                    TenantId = AbpSession.TenantId
                });
            }
            #endregion
            #endregion


            return m;
        }

        public override async Task<ProductCategoryDto> GetAsync(EntityDto<long> input)
        {
            var m = await base.GetAsync(input);
            var ls = await dynamicEntityPropertyManager.GetAllAsync(typeof(SkuEntity).FullName + input.Id);
            //var ids = ls.Select(c => c.DynamicPropertyId).Distinct();
            m.DynamicProperty = new List<DynamicPropertyEditDto>();
            foreach (var item in ls)
            {
                var t = new DynamicPropertyEditDto
                {
                    DisplayName = item.DynamicProperty.DisplayName,
                    InputType = item.DynamicProperty.InputType,
                    PropertyName = item.DynamicProperty.PropertyName.TrimStart(input.Id.ToString().ToArray())//abp默认的动态属性是全局唯一的，我们这里为了方便用户为每个类别建立自己的动态属性约束，加上id
                };
                t.DynamicPropertyValues = string.Join(',', (await valueManager.GetAllValuesOfDynamicPropertyAsync(item.DynamicPropertyId)).Select(c => c.Value));
                m.DynamicProperty.Add(t);
            }
            return m;
        }
    }
}

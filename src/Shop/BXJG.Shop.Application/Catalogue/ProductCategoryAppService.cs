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
        private readonly DynamicEntityPropertyStore dynamicEntityPropertyStore;
        private readonly DynamicPropertyManager<ProductCategoryEntity> dynamicPropertyManager;

        private readonly TempFileManager tempFileManager;
        public ProductCategoryAppService(IRepository<ProductCategoryEntity, long> ownRepository,
                                         TempFileManager tempFileManager,
                                         ProductCategoryManager organizationUnitManager,
                                         DynamicPropertyManager propertyManager,
                                         DynamicEntityPropertyStore dynamicEntityPropertyManager,
                                         DynamicPropertyManager<ProductCategoryEntity> dynamicPropertyManager,
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
            this.dynamicEntityPropertyStore = dynamicEntityPropertyManager;
            this.dynamicPropertyManager = dynamicPropertyManager;
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
            m.DynamicProperty = (await dynamicPropertyManager.SetDynamicPropertyAsync(input.DynamicProperty, m.Id)).ToDto(m.Id);
            return m;
        }

        public override async Task<ProductCategoryDto> UpdateAsync(ProductCategoryEditDto input)
        {
            if (!input.Icon.IsNullOrWhiteSpace())
                input.Icon = (await this.tempFileManager.MoveAsync(input.Icon)).Single().FileRelativePath;
            if (!input.Image1.IsNullOrWhiteSpace())
                input.Image1 = (await this.tempFileManager.MoveAsync(input.Image1)).Single().FileRelativePath;
            if (!input.Image2.IsNullOrWhiteSpace())
                input.Image2 = (await this.tempFileManager.MoveAsync(input.Image2)).Single().FileRelativePath;

            var m = await base.UpdateAsync(input);
            m.DynamicProperty = (await dynamicPropertyManager.SetDynamicPropertyAsync(input.DynamicProperty, m.Id)).ToDto(input.Id);
            return m;
        }

        public override async Task<ProductCategoryDto> GetAsync(EntityDto<long> input)
        {
            var m = await base.GetAsync(input);
            m.DynamicProperty = (await dynamicPropertyManager.GetDynamicPropertyAsync(m.Id)).ToDto(input.Id);
            return m;
        }
    }
}

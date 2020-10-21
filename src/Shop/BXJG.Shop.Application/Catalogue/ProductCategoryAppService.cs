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
        private readonly TempFileManager tempFileManager;
        public ProductCategoryAppService(IRepository<ProductCategoryEntity, long> ownRepository,
                                         TempFileManager tempFileManager,
                                         ProductCategoryManager organizationUnitManager) : base(ownRepository,
                                                                                                organizationUnitManager,
                                                                                                PermissionNames.ProductCategoryCreate,
                                                                                                PermissionNames.ProductCategoryUpdate,
                                                                                                PermissionNames.ProductCategoryDelete,
                                                                                                PermissionNames.ProductCategory)
        {
            this.tempFileManager = tempFileManager;
        }

        public override async Task<ProductCategoryDto> CreateAsync(ProductCategoryEditDto input)
        {
            if (!input.Icon.IsNullOrWhiteSpace())
                input.Icon = (await this.tempFileManager.MoveAsync(input.Icon)).Single().FileRelativePath;
            if (!input.Image1.IsNullOrWhiteSpace())
                input.Image1 = (await this.tempFileManager.MoveAsync(input.Image1)).Single().FileRelativePath;
            if (!input.Image2.IsNullOrWhiteSpace())
                input.Image2 = (await this.tempFileManager.MoveAsync(input.Image2)).Single().FileRelativePath;

            return await base.CreateAsync(input);
        }

        public override async Task<ProductCategoryDto> UpdateAsync(ProductCategoryEditDto input)
        {
            if (!input.Icon.IsNullOrWhiteSpace())
                input.Icon = (await this.tempFileManager.MoveAsync(input.Icon)).Single().FileRelativePath;
            if (!input.Image1.IsNullOrWhiteSpace())
                input.Image1 = (await this.tempFileManager.MoveAsync(input.Image1)).Single().FileRelativePath;
            if (!input.Image2.IsNullOrWhiteSpace())
                input.Image2 = (await this.tempFileManager.MoveAsync(input.Image2)).Single().FileRelativePath;

            return await base.UpdateAsync(input);
        }
    }
}

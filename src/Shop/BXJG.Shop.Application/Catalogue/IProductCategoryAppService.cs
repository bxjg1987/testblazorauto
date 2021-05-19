using Abp.Application.Services.Dto;
using Abp.Authorization;
using BXJG.GeneralTree;
using BXJG.Utils.DynamicProperty;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 后端管理对商品分类的操作
    /// </summary>
    public interface IProductCategoryAppService : IGeneralTreeAppServiceBase<ProductCategoryDto,
                                                                             ProductCategoryEditDto,
                                                                             ProductCategoryEditDto,
                                                                             BXJG.Common.Dto.BatchOperationInputLong,
                                                                             ProductCategoryGetAllInput,
                                                                             EntityDto<long>,
                                                                             GeneralTreeNodeMoveInput>
    {
        [AbpAuthorize]
        Task<IList<DynamicPropertyModel>> GetDynamicPropertyAsync(long id);
    }
}

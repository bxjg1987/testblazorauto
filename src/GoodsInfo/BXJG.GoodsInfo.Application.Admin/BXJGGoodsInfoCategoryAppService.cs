using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BXJG.Common.Dto;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo.Application.Admin
{
    /// <summary>
    /// 后台管理物品分类应用服务
    /// </summary>
    public class BXJGGoodsInfoCategoryAppService : GeneralTreeAppServiceBase<CategoryDto,
                                                                             CategoryEditDto,
                                                                             CategoryEditDto,
                                                                             BatchOperationInputLong,
                                                                             CategoryGetAllInput,
                                                                             EntityDto<long>,
                                                                             CategoryMoveInput,
                                                                             GoodsInfoCategoryEntity,
                                                                             GoodsInfoCategoryManager>
    {
        public BXJGGoodsInfoCategoryAppService(IRepository<GoodsInfoCategoryEntity, long> ownRepository,
                                               GoodsInfoCategoryManager manager) : base(ownRepository,
                                                                                        manager,
                                                                                        BXJGGoodsInfoCoreConsts.GoodsInfoCategoryCreate,
                                                                                        BXJGGoodsInfoCoreConsts.GoodsInfoCategoryUpdate,
                                                                                        BXJGGoodsInfoCoreConsts.GoodsInfoCategoryDelete,
                                                                                        BXJGGoodsInfoCoreConsts.GoodsInfoCategoryManager)
        {
        }
    }
}

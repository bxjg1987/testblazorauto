using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Localization;
using Abp.Authorization;
using Abp.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
namespace BXJG.GoodsInfo.Application.Common
{
    [AbpAuthorize]
    public class WorkOrderCategoryProviderAppService : UnAuthGeneralTreeAppServiceBase<CategoryGetForSelectInput,
                                                                                       CategoryTreeDto,
                                                                                       CategoryGetForSelectInput,
                                                                                       CategoryComboboxDto,
                                                                                       CategoryEntity>
    {
        public WorkOrderCategoryProviderAppService(IRepository<CategoryEntity, long> repository, string allTextForSearch = "不限", string allTextForForm = "请选择") : base(repository, allTextForSearch, allTextForForm)
        {
        }
    }
}

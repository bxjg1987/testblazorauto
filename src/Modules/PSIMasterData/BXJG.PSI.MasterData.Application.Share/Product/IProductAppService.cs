using Abp.Application.Services.Dto;
using BXJG.Utils.Application.Share;
using BXJG.Utils.Application.Share.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.PSI.MasterData.Application.Share.Product
{
    /// <summary>
    /// 产品应用服务接口
    /// </summary>
    public interface IProductAppService : ICrudBaseAppService<ProductDto,
                                                             string,
                                                            PagedAndSortedResultRequest<ProductCondition> ,
                                                             ProductEditDto,
                                                             ProductEditDto,
                                                             EntityDto<string>,
                                                             EntityDto<string>>
    {
    }
}

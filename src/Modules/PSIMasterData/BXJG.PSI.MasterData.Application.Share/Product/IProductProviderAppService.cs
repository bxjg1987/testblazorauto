using Abp.Application.Services.Dto;
using BXJG.Utils.Application.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.PSI.MasterData.Application.Share.Product
{
    /// <summary>
    /// 产品提供者应用服务接口
    /// 用于提供产品的下拉数据选择功能
    /// </summary>
    public interface IProductProviderAppService : IProviderBaseAppService<ProductPagedResultRequestDto,
                                                                      ProductSelectDto,
                                                                      string>
    {
    }
}

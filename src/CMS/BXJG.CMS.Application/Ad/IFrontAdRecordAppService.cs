using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.CMS.Ad
{
    /// <summary>
    /// 前台暂时广告时通过此接口来查询广告列表
    /// </summary>
    public interface IFrontAdRecordAppService : IApplicationService
    {
        Task<FrontAdPositionDto> GetAllAsync(FrontGetAdInput input);
    }
}

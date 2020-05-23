using Abp.Application.Services;
using Abp.Domain.Repositories;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Abp.Collections.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BXJG.CMS.Ad
{
    /// <summary>
    /// 前台暂时广告时通过此接口来查询广告列表
    /// </summary>
    public class FrontAdRecordAppService : ApplicationService, IFrontAdRecordAppService
    {
        protected readonly IRepository<AdRecordEntity, long> repository;

        public FrontAdRecordAppService(IRepository<AdRecordEntity, long> repository)
        {
            this.repository = repository;
        }

        public async virtual Task<FrontAdPositionDto> GetAllAsync(FrontGetAdInput input)
        {
            var ads = await repository.GetAllIncluding(c => c.Ad, c => c.AdPosition, c => c.AdControl)
                .AsNoTracking()
                .Where(c => c.AdPositionId == input.PositionId)
                .ToListAsync();

            var position = ObjectMapper.Map<FrontAdPositionDto>(ads.First().AdPosition);
            position.Controls = ObjectMapper.Map<List<FrontAdControlDto>>(ads.Select(c => c.AdControl).Distinct().ToList());
            foreach (var item in position.Controls)
            {
                item.Ads = ObjectMapper.Map<List<FrontAdRecordDto>>(ads.Where(c => c.AdControlId == item.Id));
            }
            return position;
        }
    }
}

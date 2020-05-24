using Abp.Application.Services;
using Abp.Domain.Repositories;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Abp.Collections.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace BXJG.CMS.Ad
{
    /// <summary>
    /// 前台暂时广告时通过此接口来查询广告列表
    /// </summary>
    public class BXJGCMSFrontAdRecordAppService : ApplicationService, IBXJGCMSFrontAdRecordAppService
    {
        protected readonly IRepository<AdRecordEntity, long> repository;

        public BXJGCMSFrontAdRecordAppService(IRepository<AdRecordEntity, long> repository)
        {
            base.LocalizationSourceName = BXJGCMSConsts.LocalizationSourceName;
            this.repository = repository;
        }
        /// <summary>
        /// 获取指定广告位中，已发布且在发布时间范围内的广告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async virtual Task<FrontAdPositionControlEntityDto> GetAllAsync(FrontGetAdInput input)
        {
            var n = DateTimeOffset.Now;

            //这里的查询涉及到业务，只能已发布且在时间范围内的，这个属于一条业务规则，需要放到AdManager中？
            var ads = await repository.GetAllIncluding(c => c.Ad, c => c.AdPosition, c => c.AdControl)
                .AsNoTracking()
                .Where(c => c.AdPositionId == input.PositionId && c.Published)
                .Where(c => !c.PublishStartTime.HasValue || n >= c.PublishStartTime.Value)
                .Where(c => !c.PublishEndTime.HasValue || n < c.PublishEndTime.Value)
                .ToListAsync();

            //应该在广告发布时去限制同一广告位不能同时发布多种广告控件的广告，注意处理并发问题（无法使用数据库乐观并发，考虑lock或分布式锁）
            if (ads.Select(c => c.AdControl).Count() > 1)
                throw new ApplicationException(L("同一广告位不能同时显示多种控件的广告！"));

            var position = ObjectMapper.Map<FrontAdPositionControlEntityDto>(ads.First());

            position.Ads = ObjectMapper.Map<List<FrontAdRecordDto>>(ads.Where(c => c.AdPositionId == input.PositionId));

            return position;
        }
    }
}

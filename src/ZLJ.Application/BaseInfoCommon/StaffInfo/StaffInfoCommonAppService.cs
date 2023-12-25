using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.BaseInfo.StaffInfo;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
namespace ZLJ.BaseInfoCommon.StaffInfo
{
    /// <summary>
    /// 前后台公用的，主要用来查询员工信息，登陆用户就可以访问，不验证功能权限
    /// </summary>
    [UnitOfWork(false)]
    [AbpAuthorize]
    [Obsolete("请使用common中的相应服务")]
    public class StaffInfoCommonAppService : ApplicationService
    {
        private readonly IRepository<StaffInfoEntity, long> repository;

        public StaffInfoCommonAppService(IRepository<StaffInfoEntity, long> repository)
        {
            this.repository = repository;
        }

        public async Task<PagedResultDto<Dto>> GetListAsync(GetListInput input)
        {
            var query = repository.GetAll()
                                        .AsNoTrackingWithIdentityResolution()
                                        .Include(c => c.Area)
                                        .Include(c => c.Roles)
                                        .WhereIf(!input.GetTotalInput.AreaCode.IsNullOrWhiteSpace(), c => c.Area.Code.StartsWith(input.GetTotalInput.AreaCode))
                                        .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), c => c.Name.Contains(input.Keyword)
                                                                    || c.No.Contains(input.Keyword))
                                        .WhereIf(!input.GetTotalInput.Keyword.IsNullOrWhiteSpace(), c => c.Name.Contains(input.GetTotalInput.Keyword)
                                                                    || c.No.Contains(input.GetTotalInput.Keyword)
                                                                    || c.PhoneNumber.Contains(input.GetTotalInput.Keyword));
            var r = new PagedResultDto<Dto>();
            r.TotalCount = await query.CountAsync();
            query = query.OrderBy(input.Sorting).PageBy(input);
            var list = await query.ToListAsync();
            r.Items = base.ObjectMapper.Map<IReadOnlyList<Dto>>(list);
            return r;
        }

        //public async Task<long> GetTotalAsync(GetTotalInput input)
        //{

        //}
    }
}

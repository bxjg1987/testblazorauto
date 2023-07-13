using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Organizations;
using BXJG.Common.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.BaseInfo;
using ZLJ.Customer;

namespace ZLJ.App.Common.Customer
{
    /// <summary>
    /// 公共的 获取客户部门下拉框数据
    /// </summary>
    [AbpAuthorize] //暂时用这个，以后缓存权限依赖 参考权限定义的DependencePermissions扩展方法
    [UnitOfWork(false)]
    public class OuProviderAppService : CommonBaseApplicationService
    {
        IRepository<CustomerOUEntity, long> repository;
        public OuProviderAppService(IRepository<CustomerOUEntity, long> repository)
        {
            this.repository = repository;
        }
        public virtual async Task<List<OuDto>> GetForSelectAsync(OuGetAllInput input)
        {
            var q = repository.GetAll().AsNoTrackingWithIdentityResolution()
                                //ef全局过滤器不支持继承的实体，手动来吧
                                .Where(c => c.CustomerId == input.CustomerId && c.IsActive).OrderBy(c => c.DisplayName);
            //base.Logger.Debug($"测试在应用服务上的[UnitOfWork(false)]是否有效：{base.CurrentUnitOfWork.Options.IsTransactional}");
            //base.Logger.Debug($"测试在blazor中，多次请求，当前uow是否是同一个实例：{base.CurrentUnitOfWork.GetHashCode()}");
         
            var list = await q.ToListAsync();
            return base.ObjectMapper.Map<List<OuDto>>(list);
        }
        //public virtual async Task<OuDto> GetAsync(EntityDto<long> input)
        //{
        //    var entity = await repository.GetAll().AsNoTrackingWithIdentityResolution().SingleAsync(c => c.Id == input.Id && c.CustomerId == CustomerSession.CustomerId);
        //    return await Map2Dto(entity);
        //}
    }
}

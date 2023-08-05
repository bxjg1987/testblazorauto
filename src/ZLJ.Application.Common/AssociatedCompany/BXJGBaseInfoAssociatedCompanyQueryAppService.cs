using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Castle.Core;
using Castle.Core.Logging;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZLJ.BaseInfo.AssociatedCompany;

namespace ZLJ.App.Common.AssociatedCompany
{
    public class tttt : IDisposable, ITransientDependency
    {
        public ILogger Logger { get; set; }
        public void Dispose()
        {
            Logger.Debug("测试释放了！！！");
            //throw new NotImplementedException();
        }
    }

    //[UnitOfWork(false)]
    //[AbpAuthorize]
    public class BXJGBaseInfoAssociatedCompanyQueryAppService : CommonProviderBaseAppService<AssociatedCompanyEntity, long, GetAllInput, Dto>//, IServiceProviderExAccessor
    // , IBXJGBaseInfoAssociatedCompanyQueryAppService
    {
        //private readonly IRepository<AssociatedCompanyEntity, long> _associatedCompanyRepository;
        //private readonly IAsyncQueryableExecuter _asyncQueryableExecuter;

        //public BXJGBaseInfoAssociatedCompanyQueryAppService(
        //    IRepository<AssociatedCompanyEntity, long> associatedCompanyRepository,
        //    IAsyncQueryableExecuter asyncQueryableExecuter)
        //{
        //    _associatedCompanyRepository = associatedCompanyRepository;
        //    _asyncQueryableExecuter = asyncQueryableExecuter;
        //}
        //tttt _tt;

        //public IServiceProviderEx ServiceProvider { get; set; }
        //tttt tt => _tt ??= ServiceProvider.GetService<tttt>();

        //public override Task<PagedResultDto<Dto>> GetAllAsync(GetAllInput input)
        //{
        //    var sdf = tt.GetHashCode();
        //    Logger.Debug($"_services的hashcode：{ServiceProvider.GetHashCode()}");
        //  //  base.Logger.Debug($"iocmanager的hashcode：{base.IocManager.GetHashCode()}");
        //    base.Logger.Debug($"repository的hashcode：{base.Repository.GetHashCode()}");
        //    return base.GetAllAsync(input);
        //}
        [Obsolete("请直接使用GetAllAsync")]
        public Task<ListResultDto<Dto>> GetCompaniesForSelectAsync(GetAllInput input)
        {
            return GetAllAsync(input).ContinueWith(x => new ListResultDto<Dto>(x.Result.Items));
        }
        //public async Task<ListResultDto<Dto>> GetCompaniesForSelectAsync(GetAllInput input)
        //{
        //    var query = _associatedCompanyRepository.GetAll()
        //        .AsNoTrackingWithIdentityResolution()
        //        .Where(c => c.IsActive)
        //        .WhereIf(input.LevelId.HasValue, x => x.LevelId == input.LevelId)
        //        .WhereIf(input.CategoryId.HasValue, x => x.CategoryId == input.CategoryId)
        //        .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), c => c.Name.Contains(input.Keyword) ||
        //                                                           c.TaxNo.Contains(input.Keyword) ||
        //                                                           c.LinkPhone.Contains(input.Keyword) ||
        //                                                           c.LinkMan.Contains(input.Keyword) ||
        //                                                           c.Pinyin.Contains(input.Keyword));

        //    var data = await _asyncQueryableExecuter.ToListAsync(query);
        //    var list = ObjectMapper.Map<List<Dto>>(data);
        //    foreach (var item in list.Where(x => x.Value == input.SelectedId?.ToString()))
        //    {
        //        item.IsSelected = true;
        //    }

        //    return new ListResultDto<Dto>(list);
        //}
    }
}
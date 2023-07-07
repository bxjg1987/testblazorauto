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
using Microsoft.EntityFrameworkCore;
using ZLJ.BaseInfo.AssociatedCompany;

namespace ZLJ.App.Common.AssociatedCompany
{
    [UnitOfWork(false)]
    [AbpAuthorize]
    public class BXJGBaseInfoAssociatedCompanyQueryAppService : ApplicationService
    // , IBXJGBaseInfoAssociatedCompanyQueryAppService
    {
        private readonly IRepository<AssociatedCompanyEntity, long> _associatedCompanyRepository;
        private readonly IAsyncQueryableExecuter _asyncQueryableExecuter;

        public BXJGBaseInfoAssociatedCompanyQueryAppService(
            IRepository<AssociatedCompanyEntity, long> associatedCompanyRepository,
            IAsyncQueryableExecuter asyncQueryableExecuter)
        {
            _associatedCompanyRepository = associatedCompanyRepository;
            _asyncQueryableExecuter = asyncQueryableExecuter;
        }


        public async Task<ListResultDto<Dto>> GetCompaniesForSelectAsync(GetAllInput input)
        {
            var query = _associatedCompanyRepository.GetAll()
                .AsNoTrackingWithIdentityResolution()
                .Where(c => c.IsActive)
                .WhereIf(input.LevelId.HasValue, x => x.LevelId == input.LevelId)
                .WhereIf(input.CategoryId.HasValue, x => x.CategoryId == input.CategoryId)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), c => c.Name.Contains(input.Keyword) ||
                                                                   c.TaxNo.Contains(input.Keyword) ||
                                                                   c.LinkPhone.Contains(input.Keyword) ||
                                                                   c.LinkMan.Contains(input.Keyword) ||
                                                                   c.Pinyin.Contains(input.Keyword));

            var data = await _asyncQueryableExecuter.ToListAsync(query);
            var list = ObjectMapper.Map<List<Dto>>(data);
            foreach (var item in list.Where(x => x.Value == input.SelectedId?.ToString()))
            {
                item.IsSelected = true;
            }

            return new ListResultDto<Dto>(list);
        }
    }
}
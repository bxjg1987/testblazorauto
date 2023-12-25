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
using ZLJ.BaseInfo.AssociatedCompany.Dto;
using ZLJ.BaseInfo.Dto;
using Microsoft.EntityFrameworkCore;
using System;

namespace ZLJ.BaseInfo.AssociatedCompany
{
    [Obsolete("ÒÑ¾­ÒÆÖ²µ½common")]
    [AbpAuthorize]
    public class BXJGBaseInfoAssociatedCompanyQueryAppService : ApplicationService,
        IBXJGBaseInfoAssociatedCompanyQueryAppService
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

        [UnitOfWork(false)]
        public async Task<ListResultDto<AssociatedCompanyGetForSelectOutputDto>> GetCompaniesForSelectAsync(
            ComboboxInputDto input)
        {
            var query = _associatedCompanyRepository.GetAll()
                .AsNoTrackingWithIdentityResolution()
                .WhereIf(!input.Q.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Q));

            var data = await _asyncQueryableExecuter.ToListAsync(query);
            var list = ObjectMapper.Map<List<AssociatedCompanyGetForSelectOutputDto>>(data);
            foreach (var item in list.Where(x => x.Value == input.SelectedId?.ToString()))
            {
                item.IsSelected = true;
            }

            return new ListResultDto<AssociatedCompanyGetForSelectOutputDto>(list);
        }
    }
}
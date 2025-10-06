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
using BXJG.Utils.Application.Share;
using BXJG.Utils.Application.Share.Dtos;
using Castle.Core;
using Castle.Core.Logging;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZLJ.Application.Common.Share.Kehu;
using ZLJ.Core.AssociatedCompany;

namespace ZLJ.Application.Common.AssociatedCompany
{
    //[UnitOfWork(false)]
    [AbpAuthorize]
    public class KehuProviderAppService : CommonProviderBaseAppService<AssociatedCompanyEntity,
                                                                                             PagedAndSortedResultRequest<Condition>,
                                                                                             KehuDto,
                                                                                             long>, IKehuProviderAppService
    {
        protected override async Task< IQueryable<AssociatedCompanyEntity>> BuildQuery()
        {
            return (await base.BuildQuery()).Include(x => x.Level).Include(x => x.Area);
        }

        protected override async Task<IQueryable<AssociatedCompanyEntity>> CreateFilteredQuery(PagedAndSortedResultRequest<Condition> input)
        {
            return (await base.CreateFilteredQuery(input)).WhereIf(input.Filter.LevelId.HasValue,x=>x.LevelId==input.Filter.LevelId)
                                                  .WhereIf(input.Filter.Keywords.IsNotNullOrWhiteSpaceBXJG(), x => x.Pinyin.Contains(input.Filter.Keywords) ||
                                                                                                                  x.Name.Contains(input.Filter.Keywords) ||
                                                                                                                  x.LinkMan.Contains(input.Filter.Keywords) ||
                                                                                                                  x.LinkPhone.Contains(input.Filter.Keywords) ||
                                                                                                                  x.Address.Contains(input.Filter.Keywords) ||
                                                                                                                  x.AddressPinyin.Contains(input.Filter.Keywords) ||
                                                                                                                  x.LinkManPinyin.Contains(input.Filter.Keywords));
        }
    }
}
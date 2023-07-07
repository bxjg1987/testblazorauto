using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ZLJ.BaseInfo.AssociatedCompany.Dto;
using ZLJ.BaseInfo.Dto;
using System;
namespace ZLJ.Application.Common.AssociatedCompany
{
    [Obsolete("ÒÑ¾­ÒÆÖ²µ½common")]
    public interface IBXJGBaseInfoAssociatedCompanyQueryAppService : IApplicationService
    {
        Task<ListResultDto<AssociatedCompanyGetForSelectOutputDto>> GetCompaniesForSelectAsync(
            ComboboxInputDto input);
    }
}
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace ZLJ.App.Common.AssociatedCompany
{
    public interface IBXJGBaseInfoAssociatedCompanyQueryAppService : IApplicationService
    {
        Task<ListResultDto<Dto>> GetCompaniesForSelectAsync(GetAllInput input);
    }
}
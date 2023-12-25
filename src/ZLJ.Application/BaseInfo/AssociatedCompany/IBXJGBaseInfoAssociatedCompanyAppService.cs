using System.Threading.Tasks;
using Abp.Application.Services;
using ZLJ.App.Admin.BaseInfo.AssociatedCompany.Dto;
using ZLJ.App.Admin.BaseInfo.StaffInfo;
using BXJG.Common.Dto;

namespace ZLJ.App.Admin.BaseInfo.AssociatedCompany
{
    /// <summary>
    /// 基础信息-来往单位
    /// </summary>
    public interface IBXJGBaseInfoAssociatedCompanyAppService :
        IAsyncCrudAppService<AssociatedCompanyDto, long, AssociatedCompanyGetAllInput, AssociatedCompanyEditDto>
    {
        /// <summary>
        /// 批量删除
        /// </summary>
        Task<BatchOperationOutputLong> DeleteBatchAsync(BatchOperationInputLong input);
    }
}
using System.Threading.Tasks;
using Abp.Application.Services;
using ZLJ.BaseInfo.StaffInfo.Dto;
using BXJG.Common.Dto;

namespace ZLJ.BaseInfo.StaffInfo
{
    /// <summary>
    /// 基础信息-员工档案
    /// </summary>
    public interface IBXJGBaseInfoStaffInfoAppService : IAsyncCrudAppService<StaffInfoDto, long, StaffInfoGetAllInput, StaffInfoEditDto>
    {
        /// <summary>
        /// 批量删除
        /// </summary>
        Task<BatchOperationOutputLong> DeleteBatchAsync(BatchOperationInputLong input);
    }
}
using Abp.Application.Services;
using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
namespace ZLJ.Utils.Application
{
    /// <summary>
    /// 系统枚举
    /// </summary>
    public interface IUtilsAppService : IApplicationService
    {
        /// <summary>
        /// 根据名称获取枚举
        /// </summary>
        /// <param name="enumTypeName"></param>
        /// <returns></returns>
        [AbpAuthorize]
        IList<ComboboxItemDto> GetByName(GetEnumByNameInput input);
        /// <summary>
        /// 获取性别
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize]
        IList<ComboboxDto<int>> GetGender(NullableInput input);
        /// <summary>
        /// 组织单位类型
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize]
        IList<ComboboxDto<int>> GetOUType(NullableInput input );
        /// <summary>
        /// 星期一至星期天
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize]
        IList<ComboboxDto<int>> GetWeekDay(NullableInput input);

        /// <summary>
        /// 行政级别
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize]
        IList<ComboboxDto<int>> GetAdministrativeLevel(NullableInput input);

        //由于abp可以为应用服务生成js代理，客服端调用更方便

        /// <summary>
        /// 获取拼音简码
        /// </summary>
        /// <param name="chinese"></param>
        /// <param name="full"></param>
        /// <param name="tolower"></param>
        /// <returns></returns>
        [AbpAuthorize]
        string GetPY(GetPYInput input);

        [AbpAuthorize]
        IList<ComboboxDto<int>> GetBool(NullableInput input);
    }
}

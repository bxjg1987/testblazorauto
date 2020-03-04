using Abp.Application.Services;
using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
namespace BXJG.Utils.Application
{
    /// <summary>
    /// 系统枚举
    /// </summary>
    public interface IBXJGUtilsAppService : IApplicationService
    {
        IList<ComboboxItemDto> GetByName(GetEnumForCombboxInput input);
        //IList<ComboboxDto<int>> GetGender(NullableInput input);
        //IList<ComboboxDto<int>> GetOUType(NullableInput input );
        //IList<ComboboxDto<int>> GetWeekDay(NullableInput input);
        //IList<ComboboxDto<int>> GetAdministrativeLevel(NullableInput input);

        ////由于abp可以为应用服务生成js代理，客服端调用更方便

       
        //string GetPY(GetPYInput input);

        //IList<ComboboxDto<int>> GetBool(NullableInput input);
    }
}

using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ
{
    public interface IUtilsAppService : Abp.Application.Services.IApplicationService
    {
        [AbpAuthorize]
        string GetPinYinFirstLetter(string chinese, bool toUpper = true);
    }
}

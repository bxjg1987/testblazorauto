using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.Share.OU
{
    public interface IOuAppService : IApplicationService
    {
        Task<IList<OuDto>> GetListAsync(GetListInput input);
    }
}

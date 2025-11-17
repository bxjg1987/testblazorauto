using Abp.Application.Services;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.Application.Share.OU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.Share.OU
{
    public interface IOUSelectProviderAppService : IGeneralTreeProviderBaseAppService<GetListInput, OUSelectDto, GeneralTreeGetForSelectInput,
                                                                               GeneralTreeComboboxDto>
    {
        //Task<IList<OuDto>> GetListAsync(GetListInput input);
    }
}

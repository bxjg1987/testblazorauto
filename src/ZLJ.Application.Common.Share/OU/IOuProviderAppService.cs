using Abp.Application.Services;
using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.Share.OU
{
    public interface IOuProviderAppService : IGeneralTreeProviderBaseAppService<GetListInput, OuDto, GeneralTreeGetForSelectInput,
                                                                               GeneralTreeComboboxDto>
    {
        //Task<IList<OuDto>> GetListAsync(GetListInput input);
    }
}

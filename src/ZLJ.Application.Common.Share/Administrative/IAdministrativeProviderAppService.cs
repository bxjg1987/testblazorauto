using Abp.Application.Services;
using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.Share.Administrative
{
    public interface IAdministrativeProviderAppService : IGeneralTreeProviderBaseAppService<GetListInput, AdministrativeDto, GeneralTreeGetForSelectInput, GeneralTreeComboboxDto>
    {
        //Task<IList<OuDto>> GetListAsync(GetListInput input);
    }
}

using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.Administrative;

namespace ZLJ.RCL.Components
{
    public class AdministrativeTreeSelect : TreeSelect<GetListInput, AdministrativeDto, GeneralTreeGetForSelectInput, GeneralTreeComboboxDto,
                                                       IAdministrativeProviderAppService>
    {
        protected override Task OnInitializedAsync()
        {
            if (Placeholder.IsNullOrWhiteSpaceBXJG())
            {
                Placeholder = "请选择省市区县";
            }
            return base.OnInitializedAsync();
        }
    }
}

using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.Settings
{
    public class SettingDefinitionGroupDto<TChild>: GeneralTreeNodeBaseDto<TChild>
        where TChild : SettingDefinitionGroupDto<TChild>
    {
        //public string Name { get; set; }
        //public string DisplayName { get; set; }
        //public List<SettingDto> Items { get; set; }
    }

    public class SettingDefinitionGroupDto : SettingDefinitionGroupDto<SettingDefinitionGroupDto>
    {
        //public string Name { get; set; }
        //public string DisplayName { get; set; }
        //public List<SettingDto> Items { get; set; }
    }
}

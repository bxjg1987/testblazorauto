using Abp.AutoMapper;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Common.Dto
{
    public class DictionaryDto : GeneralTreeGetTreeNodeBaseDto<DictionaryDto>
    {
        public string Icon { get; set; }
    }
}

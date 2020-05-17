using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Common.Dto
{
    public class DictionaryTreeNodeDto : GeneralTreeNodeDto<DictionaryTreeNodeDto>
    {
        public string Icon { get; set; }
        public bool IsSysDefine { get; set; }
        public bool IsTree { get; set; }
    }
}

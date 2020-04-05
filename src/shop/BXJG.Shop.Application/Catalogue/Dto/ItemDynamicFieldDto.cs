using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue.Dto
{
    public class ItemDynamicFieldDto : GeneralTreeGetTreeNodeBaseDto<ItemDynamicFieldDto>
    {
        public string Icon { get; set; }
    }
}

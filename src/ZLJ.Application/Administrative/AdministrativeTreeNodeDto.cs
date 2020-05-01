using BXJG.GeneralTree;
using BXJG.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZLJ.Administrative
{
    public class AdministrativeTreeNodeDto : GeneralTreeNodeDto<AdministrativeTreeNodeDto>
    {
        public AdministrativeLevel Level { get; set; }
    }
}

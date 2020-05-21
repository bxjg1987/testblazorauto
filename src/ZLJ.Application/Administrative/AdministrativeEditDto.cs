using BXJG.Common;
using BXJG.GeneralTree;
using BXJG.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZLJ.Administrative
{
    public class AdministrativeEditDto : GeneralTreeNodeEditBaseDto
    {
        public AdministrativeLevel Level { get; set; }
    }
}

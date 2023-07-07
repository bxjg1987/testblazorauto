using BXJG.Utils.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Localization;

namespace ZLJ.App.Admin.OU
{
    public class OUDto: GeneralTreeGetTreeNodeBaseDto<OUDto>
    {  /// <summary>
       /// 0总公司 1分公司 2部门
       /// </summary>
        public Enums.OUType OUType { get; set; }
        public string OUTypeText => OUType.Enum();
    }
}

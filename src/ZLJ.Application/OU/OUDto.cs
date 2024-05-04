using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Share;
using ZLJ.Core.Localization;

namespace ZLJ.Application.OU
{
    public class OUDto: GeneralTreeNodeBaseDto<OUDto>
    {  /// <summary>
       /// 0总公司 1分公司 2部门
       /// </summary>
        public OUType OUType { get; set; }
        public string OUTypeText => OUType.Enum();
    }
}

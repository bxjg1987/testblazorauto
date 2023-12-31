using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Share;

namespace ZLJ.App.Admin.OU
{
    public class OUEditDto:GeneralTreeNodeEditBaseDto
    {  /// <summary>
       /// 0总公司 1分公司 2部门
       /// </summary>
        public OUType OUType { get; set; }
    }
}

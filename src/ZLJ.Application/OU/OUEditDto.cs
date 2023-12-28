using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.App.Admin.OU
{
    public class OUEditDto:GeneralTreeNodeEditBaseDto
    {  /// <summary>
       /// 0总公司 1分公司 2部门
       /// </summary>
        public Enums.OUType OUType { get; set; }
    }
}

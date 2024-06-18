using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Share;

namespace ZLJ.Application.Share.OU
{
    public class OUEditDto:GeneralTreeNodeEditBaseDto
    {  /// <summary>
       /// 0总公司 1分公司 2部门
       /// </summary>
        [Required]
        public OUType OUType { get; set; }
    }
}

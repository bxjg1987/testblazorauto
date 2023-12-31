using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Share;

namespace ZLJ.Application.Common.Share.OU
{
    /// <summary>
    /// 获取公司和部门下拉树的输出模型
    /// </summary>
    public class OuDto : GeneralTreeNodeDto<OuDto>
    {
        /// <summary>
        /// 0总公司 1分公司 2部门
        /// </summary>
        public OUType OUType { get; set; }
        public string OUTypeText { get; set; }

    }
}

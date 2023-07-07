using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Localization;

namespace ZLJ.App.Common.OU
{
    /// <summary>
    /// 获取公司和部门下拉树的输出模型
    /// </summary>
    public class OuDto : BXJG.Utils.GeneralTree.GeneralTreeNodeDto<OuDto>
    {
        /// <summary>
        /// 0总公司 1分公司 2部门
        /// </summary>
        public Enums.OUType OUType { get; set; }
        public string OUTypeText => OUType.Enum();

    }
}

using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.BaseInfo.Administrative;

namespace ZLJ.Application.Common.Administrative
{
    /// <summary>
    /// 工单类别下拉树形数据源显示模型
    /// </summary>
    public class AdministrativeTreeNodeDto : GeneralTreeNodeForSelectDto<AdministrativeTreeNodeDto>
    {
        public AdministrativeLevel Level { get; set; }
    }
}

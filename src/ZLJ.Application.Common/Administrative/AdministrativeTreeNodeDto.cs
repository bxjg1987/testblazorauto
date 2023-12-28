using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.BaseInfo.Administrative;

namespace ZLJ.App.Common.Administrative
{
    /// <summary>
    /// 工单类别下拉树形数据源显示模型
    /// </summary>
    public class AdministrativeTreeNodeDto : GeneralTreeNodeDto<AdministrativeTreeNodeDto>
    {
        public AdministrativeLevel Level { get; set; }
    }
}

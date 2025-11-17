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
    public class OUSelectDto<T> : BXJG.Utils.Application.Share.OU.OUSelectDto<T>
        where T : OUSelectDto<T>
    {
        public OUType OUType { get; set; }
        public string OUTypeText => OUType.GetDescription();
    }
    public class OUSelectDto: OUSelectDto<OUSelectDto>
    {
    }
}

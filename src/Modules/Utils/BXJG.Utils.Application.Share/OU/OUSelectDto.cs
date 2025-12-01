using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.OU
{
    /// <summary>
    /// 获取公司和部门下拉树的输出模型
    /// </summary>
    public class OUSelectDto<TChild> : GeneralTreeNodeForSelectDto<TChild>
        where TChild : OUSelectDto<TChild>
    {
    }
}

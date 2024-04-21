using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.OU;

namespace ZLJ.RCL.Components
{
    /// <summary>
    /// 部门选择下拉框
    /// </summary>
    public class TreeSelectOu : TreeSelectZlj<OuDto>
    {

        protected override Task OnInitializedAsync()
        {
            if (Placeholder.IsNullOrWhiteSpaceBXJG())
            {
                Placeholder = "请选择部门";
            }
            return base.OnInitializedAsync();
        }
    }
}
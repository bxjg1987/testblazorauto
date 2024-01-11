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
    public class TsOu : AbpTreeSelect<GetListInput,
                                      OuDto,
                                      GeneralTreeGetForSelectInput,
                                      GeneralTreeComboboxDto,
                                      IOuProviderAppService>
    {
        protected override string sy => "请选择部门";
    }
}
using Abp.AutoMapper;
using BXJG.Utils.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Localization;

namespace ZLJ.App.Common.Customer
{
    /// <summary>
    /// 公共的 获取客户部门下拉框数据的 返回模型
    /// </summary>
   // [AutoMapFrom(typeof(ZLJ.Customer.CustomerOUEntity))]
    public class OuDto: GeneralTreeNodeDto
    {
    }
}

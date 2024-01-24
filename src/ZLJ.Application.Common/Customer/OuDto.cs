using Abp.AutoMapper;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Localization;

namespace ZLJ.Application.Common.Customer
{
    /// <summary>
    /// 公共的 获取客户部门下拉框数据的 返回模型
    /// </summary>
   // [AutoMapFrom(typeof(ZLJ.Core.Customer.CustomerOUEntity))]
    public class OuDto: DataDictionaryForSelectDto
    {
    }
}

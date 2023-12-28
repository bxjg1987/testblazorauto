using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.App.Common.OU
{
    /// <summary>
    /// 获取公司和部门下拉树的输入模型
    /// </summary>
    public class GetListInput:GeneralTreeGetForSelectInput
    {
        /// <summary>
        /// 0获取我们自己公司的部门
        /// 1获取租赁客户公司的部门
        /// </summary>
       // [Obsolete("请调用OuProviderAppService")]
        public int WhatType { get; set; } = 0;
    }
}

using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.Dto
{
    /// <summary>
    /// 获取下拉框数据的输入模型
    /// </summary>
    public class GetForCombboxInput
    {
        /// <summary>
        /// 是否填充一个“==请选择==”的节点
        /// </summary>
        public bool LoadParent { get; set; } = true;
        /// <summary>
        /// 若LoadParent为true，先尝试以ParentText，若为空则“==请选择==”。注意此字段对应本地化系统的key
        /// 否则忽略此属性
        /// </summary>
        public string ParentText { get; set; }
    }
}

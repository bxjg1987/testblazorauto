using BXJG.Common;
using BXJG.GeneralTree;
using BXJG.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.Localization;

namespace ZLJ.Administrative
{
    /// <summary>
    /// 行政区域后台管理 列表页的显示模型
    /// </summary>
    public class AdministrativeDto : GeneralTreeGetTreeNodeBaseDto<AdministrativeDto>
    {
        /// <summary>
        /// 行政区域级别
        /// </summary>
        public AdministrativeLevel Level { get; set; }
        /// <summary>
        /// 获取行政区域级别本地化文本
        /// </summary>
        public string LevelText
        {
            get
            {
                return Level.Enum();
            }
        }
    }
}

using BXJG.Utils.GeneralTree;
using BXJG.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZLJ.Core.BaseInfo.Administrative
{
    /// <summary>
    /// 行政区域实体类
    /// </summary>
    public class AdministrativeEntity : GeneralTreeEntity<AdministrativeEntity>
    {
        /// <summary>
        /// 主程序的行政级别
        /// </summary>
        public AdministrativeLevel Level { get; set; }
    }
  
}

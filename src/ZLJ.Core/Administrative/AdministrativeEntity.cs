using BXJG.GeneralTree;
using BXJG.Shop.Common;
using BXJG.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZLJ.Administrative
{
    /// <summary>
    /// 行政区域实体类
    /// </summary>
    public class AdministrativeEntity : GeneralTreeEntity<AdministrativeEntity>, IShopAdministrative
    {
        /// <summary>
        /// 主程序的行政级别
        /// </summary>
        public AdministrativeLevel Level { get; set; }
    }
}

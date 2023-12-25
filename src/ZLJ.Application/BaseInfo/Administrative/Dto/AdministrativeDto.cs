using ZLJ.Localization;
using BXJG.Utils.GeneralTree;
using ZLJ.App.Admin.BaseInfo.Administrative;
using ZLJ.BaseInfo.Administrative;

namespace ZLJ.App.Admin.BaseInfo.Administrative.Dto
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
        public string LevelText => Level.Enum();
    }
}
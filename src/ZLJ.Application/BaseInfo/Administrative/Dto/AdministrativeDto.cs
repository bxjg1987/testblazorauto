using ZLJ.Core.Localization;
using BXJG.Utils.GeneralTree;
using ZLJ.Application.Admin.BaseInfo.Administrative;
using ZLJ.Core.BaseInfo.Administrative;
using BXJG.Utils.Application.Share.GeneralTree;

namespace ZLJ.Application.Admin.BaseInfo.Administrative.Dto
{
    /// <summary>
    /// 行政区域后台管理 列表页的显示模型
    /// </summary>
    public class AdministrativeDto : GeneralTreeNodeBaseDto<AdministrativeDto>
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
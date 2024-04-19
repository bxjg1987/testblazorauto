using BXJG.Utils.Application.Share.GeneralTree;
using ZLJ.Core.Share.Enums;

namespace ZLJ.Application.Share.Administrative
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

        ///// <summary>
        ///// 获取行政区域级别本地化文本
        ///// </summary>
        //public string LevelText => Level.Enum();
    }
}
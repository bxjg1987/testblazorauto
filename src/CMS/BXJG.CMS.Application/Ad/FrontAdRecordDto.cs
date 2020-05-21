using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Ad
{
    /// <summary>
    /// 前端显示广告时的查询模型
    /// </summary>
    public class FrontAdRecordDto:EntityDto<long>
    {
        #region 控件信息
        public long AdControlId { get; set; }
        /// <summary>
        /// 控件类型
        /// </summary>
        public AdControlType AdControlType { get; set; }
        /// <summary>
        /// ExtensionData以json方式存储着不同控件类型的参数，比如轮播控件中的 轮播速度、单图显示时长、轮播方式（渐变/滑动等）
        /// </summary>
        public string ExtensionData { get; set; }
        #endregion
        #region 广告位
        /// <summary>
        /// 广告位Id
        /// </summary>
        public long AdPositionId { get; set; }
        /// <summary>
        /// 宽度，显示广告时优先使用多对多关系中的尺寸，若没有才会采用此尺寸
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 高度，显示广告时优先使用多对多关系中的尺寸，若没有才会采用此尺寸
        /// </summary>
        public int Height { get; set; }
        #endregion
        #region 广告记录信息
        ///// <summary>
        ///// 是否已发布
        ///// </summary>
        //public bool Published { get; set; }
        ///// <summary>
        ///// 发布开始时间，若为空则不限
        ///// Published为true时才有效
        ///// </summary>
        //public DateTimeOffset? PublishStartTime { get; set; }
        ///// <summary>
        ///// 发布结束时间，若为空则不限
        ///// Published为true时才有效
        ///// </summary>
        //public DateTimeOffset? PublishEndTime { get; set; }
        ///// <summary>
        ///// 排序索引
        ///// </summary>
        //public int SortIndex { get; set; }
        #endregion
        #region 广告
        /// <summary>
        /// 一个控件、广告位置可能包含多个广告条目，这个属性就是这些条目
        /// </summary>
        public List<FrontAdDto> Items { get; set; }
        #endregion
    }
}

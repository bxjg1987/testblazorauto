using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Ad
{
    /// <summary>
    /// 前端查询广告时使用的模型
    /// </summary>
    public class FrontAdPositionControlEntityDto
    {
        /// <summary>
        /// 广告位Id
        /// </summary>
        public long AdPositionId { get; set; }
        /// <summary>
        /// 广告位宽度
        /// </summary>
        public int AdPositionWidth { get; set; }
        /// <summary>
        /// 广告位高度
        /// </summary>
        public int AdPositionHeight { get; set; }
        /// <summary>
        /// 控件Id
        /// </summary>
        public long AdControlId { get; set; }
        /// <summary>
        /// 控件类型
        /// </summary>
        public AdControlType AdControlAdControlType { get; set; }
        /// <summary>
        /// 控件参数
        /// </summary>
        public dynamic AdControlExtensionData { get; set; }
        /// <summary>
        /// 包含的广告列表
        /// </summary>
        public List<FrontAdRecordDto> Ads { get; set; }
    }
    /// <summary>
    /// 前端查询广告时的广告明细
    /// </summary>
    public class FrontAdRecordDto
    {
        /// <summary>
        /// 广告发布记录的Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 广告Id
        /// </summary>
        public long AdId { get; set; }
        /// <summary>
        /// 广告标题
        /// </summary>
        public string AdTitle { get; set; }
        /// <summary>
        /// 广告类型
        /// </summary>
        public AdType AdAdType { get; set; }
        /// <summary>
        /// 广告内容，不同类型的广告、内容表现不同
        /// 比如图片类型的广告，则内容为图片地址；html类型的广告内容为Html代码
        /// </summary>
        public string AdContent { get; set; }
        /// <summary>
        /// 点击广告后的连接地址
        /// </summary>
        public string AdUrl { get; set; }
        /// <summary>
        /// 多个广告的排序索引
        /// </summary>
        public int AdSortIndex { get; set; }
    }
}

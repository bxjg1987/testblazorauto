using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Ad
{
    /// <summary>
    /// 前端展示广告时使用的模型
    /// 通常广告接口返回FrontAdRecordDto，它包含广告位、控件和多个FrontAdDto
    /// </summary>
    public class FrontAdDto : EntityDto<long>
    {
        /// <summary>
        /// 广告记录Id
        /// </summary>
        public long RecordId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 广告类型，图片、文本、Html、等
        /// </summary>
        public AdType AdType { get; set; }
        /// <summary>
        /// 广告内容，不同类型的广告 广告内容的含义不同
        /// 图片：图片地址
        /// 文本：广告文本
        /// Html：Html代码
        /// Javascript：js代码
        /// JavsScriptLink：要引用的js文件地址
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 点击广告的连接地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 排序索引
        /// </summary>
        public int SortIndex { get; set; }
    }
}

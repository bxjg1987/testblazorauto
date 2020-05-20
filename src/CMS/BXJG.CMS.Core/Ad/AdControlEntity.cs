using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BXJG.CMS.Ad
{
    /// <summary>
    /// 广告控件
    /// 
    /// </summary>
    public class AdControlEntity : FullAuditedEntity, IMayHaveTenant, IExtendableObject
    {
        public int? TenantId { get; set; }
        /// <summary>
        /// 控件类型
        /// </summary>
        public AdControlType AdControlType { get; set; }
        /// <summary>
        /// ExtensionData以json方式存储着不同控件类型的参数，比如轮播控件中的 轮播速度、单图显示时长、轮播方式（渐变/滑动等）
        /// </summary>
        public string ExtensionData { get; set; }
        //public int Width { get; set; }
        //public int Height { get; set; }
    }
}

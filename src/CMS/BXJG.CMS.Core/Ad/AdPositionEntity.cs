using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Ad
{
    /// <summary>
    /// 广告位实体类
    /// </summary>
    public class AdPositionEntity : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        /// <summary>
        /// 广告位名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 宽度，显示广告时优先使用多对多关系中的尺寸，若没有才会采用此尺寸
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 高度，显示广告时优先使用多对多关系中的尺寸，若没有才会采用此尺寸
        /// </summary>
        public int Height { get; set; }
    }
}

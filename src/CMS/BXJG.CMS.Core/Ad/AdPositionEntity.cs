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
    public class AdPositionEntity : FullAuditedEntity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        /// <summary>
        /// 广告位名称
        /// </summary>
        public string DisplayName { get; set; }
    }
}

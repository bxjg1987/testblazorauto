using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Ad
{
    /// <summary>
    /// 已经或即将发布的广告，建立广告、控件、广告位之间的多对多关系
    /// </summary>
    public class AdRecordEntity : FullAuditedEntity<long>, IMustHaveTenant 
    {
        public int TenantId { get; set; }
        /// <summary>
        /// 广告Id
        /// </summary>
        public long AdId { get; set; }
        /// <summary>
        /// 关联的广告实体
        /// </summary>
        public virtual AdEntity Ad { get; set; }
        /// <summary>
        /// 广告位Id
        /// </summary>
        public long AdPositionId { get; set; }
        /// <summary>
        /// 关联的广告位
        /// </summary>
        public virtual AdPositionEntity AdPosition { get; set; }
        /// <summary>
        /// 广告控件Id
        /// </summary>
        public long AdControlId { get; set; }
        /// <summary>
        /// 关联的广告控件
        /// </summary>
        public virtual AdControlEntity AdControl { get; set; }
        /// <summary>
        /// 是否已发布
        /// </summary>
        public bool Published { get; set; }
        /// <summary>
        /// 发布开始时间，若为空则不限
        /// Published为true时才有效
        /// </summary>
        public DateTimeOffset? PublishStartTime { get; set; }
        /// <summary>
        /// 发布结束时间，若为空则不限
        /// Published为true时才有效
        /// </summary>
        public DateTimeOffset? PublishEndTime { get; set; }
        /// <summary>
        /// 排序索引
        /// </summary>
        public int SortIndex { get; set; }
    }
}

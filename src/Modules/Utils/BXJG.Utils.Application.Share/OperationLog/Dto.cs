using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.EntityHistory;
using Abp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Application.Services.Dto;
using Abp.Linq;
using Abp.Authorization;
using System.ComponentModel.DataAnnotations;
using Abp.Linq.Extensions;
using Abp.Domain.Uow;
using BXJG.Common.Contracts;

namespace BXJG.Utils.Application.Share.OperationLog
{
    /// <summary>
    /// 操作日志dto
    /// </summary>
    public class Dto<TPropertyDto> : IExtendableObj
    {
        /// <summary>
        /// 操作时的浏览器信息
        /// </summary>
        public virtual string SetBrowserInfo { get; set; }
        /// <summary>
        /// 客户端id
        /// </summary>
        public virtual string SetClientIpAddress { get; set; }
        /// <summary>
        /// 客户端名称
        /// </summary>
        public virtual string SetClientName { get; set; }
        /// <summary>
        /// 操作员id
        /// </summary>
        public virtual long? SetUserId { get; set; }
        /// <summary>
        /// 操作员姓名
        /// </summary>
        public virtual string UserName { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public virtual string SetReason { get; set; }
        /// <summary>
        /// 扩展数据
        /// </summary>
        public virtual dynamic ExtensionData { get; set; }
        /// <summary>
        /// 被操作的实体类型
        /// </summary>
        public virtual string EntityEntityTypeFullName { get; set; }
        /// <summary>
        /// 实体名
        /// </summary>
        public virtual string EntityTypeDisplayName { get; set; }
        /// <summary>
        /// 被操作的实体的id
        /// </summary>
        public virtual string EntityEntityId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public virtual DateTimeOffset EntityChangeTime { get; set; }
        /// <summary>
        /// 被改过的字段
        /// </summary>
        public virtual List<TPropertyDto> EntityPropertyChanges { get; set; }
    }

    /// <summary>
    /// 操作日志字段dto
    /// </summary>
    public class PropertyDto
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public virtual string PropertyName { get; set; }
        /// <summary>
        /// 字段显示名
        /// </summary>
        public virtual string PropertyDisplayName { get; set; }
        /// <summary>
        /// 字段类型名
        /// </summary>
        public virtual string PropertyTypeFullName { get; set; }
        /// <summary>
        /// 修改前的值
        /// </summary>
        public virtual string NewValue { get; set; }
        /// <summary>
        /// 修改前的值
        /// </summary>
        public virtual string NewValueDisplayName { get; set; }
        /// <summary>
        /// 修改后的值
        /// </summary>
        public virtual string OriginalValue { get; set; }
        /// <summary>
        /// 修改后的值
        /// </summary>
        public virtual string OriginalValueDisplayName { get; set; }
    }

    /// <summary>
    /// 获取操作日志时的输入模型
    /// </summary>
    public class GetAllInput
    {
        /// <summary>
        /// 实体id
        /// </summary>
        //[Required]
        //public string EntityTypeFullName { get; set; }
        [Required]
        public string EntityId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTimeOffset? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTimeOffset? EndTime { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public string Sorting { get; set; }
    }

    /// <summary>
    /// 查询时临时用的
    /// </summary>
    public class EntitySet
    {
        public EntityChange Entity { get; set; }
        public EntityChangeSet Set { get; set; }
    }

}

using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.Feedback
{
    /// <summary>
    /// 留言反馈查询模型
    /// </summary>
    public class FeedbackDto : FeedbackEditDto,IExtendableObj//,IExtendableObject
    {
        public dynamic? ExtensionData { get; set; }
        //string IExtendableObject.ExtensionData { get; set; }

        #region FullAuditedEntityDto
        public bool IsDeleted { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        #endregion
    }
}

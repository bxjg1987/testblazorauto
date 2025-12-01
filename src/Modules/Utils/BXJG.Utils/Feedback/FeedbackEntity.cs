using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Feedback
{
    /// <summary>
    /// 通用留言功能
    /// 留言的回复将来使用通用实体评论功能实现
    /// </summary>
    public class FeedbackEntity : FullAuditedEntity<Guid>, IMayHaveTenant, IExtendableObject
    {
        /// <summary>
        ///租户id，可选
        /// </summary>
        public int? TenantId { get; set; }

        //留言人用创建人id

        /// <summary>
        /// 称呼
        /// </summary>
        public string? ConnectName { get; set; }
        /// <summary>
        /// 联系方式
        /// 如：手机号17723345454 或者 邮箱 17723345454@163.com
        /// 有时候匿名用户留言时，没有关联的用户id，所以需要这里存储联系方式
        /// </summary>
        public string? ConnectInfo { get; set; }
        /// <summary>
        /// 标题，可选，将来可能实现大模型根据内容自动生成，目前可暂时忽略
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// 留言内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// abp提供的json形式的扩展数据
        /// </summary>
        public string? ExtensionData { get; set; }


        //标签将来使用通用Tag功能实现，目前忽略
        //若考虑图片，到时候使用通用附件功能实现，暂时忽略
    }
}

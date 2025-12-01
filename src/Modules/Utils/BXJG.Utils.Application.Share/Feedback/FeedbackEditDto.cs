using Abp.Application.Services.Dto;
using BXJG.Utils.Share;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.Feedback
{
    /// <summary>
    /// 留言反馈的编辑模型
    /// </summary>
    public class FeedbackEditDto:EntityDto<Guid>
    {
        /// <summary>
        /// 称呼
        /// </summary>
        [Display(Name = "称呼")]
        [StringLength(BXJGUtilsConsts.FeedbackConnectNameMaxLength)]
        public string? ConnectName { get; set; }
        /// <summary>
        /// 联系方式
        /// 如：手机号17723345454 或者 邮箱 17723345454@163.com
        /// 有时候匿名用户留言时，没有关联的用户id，所以需要这里存储联系方式
        /// </summary>
        [Display(Name = "联系方式")]
        [StringLength(BXJGUtilsConsts.FeedbackConnectInfoMaxLength)]
        public string? ConnectInfo { get; set; }
        /// <summary>
        /// 标题，可选，将来可能实现大模型根据内容自动生成，目前可暂时忽略
        /// </summary>
        [Display(Name = "标题")]
        [StringLength(BXJGUtilsConsts.FeedbackTitleMaxLength)]
        public string? Title { get; set; }
        /// <summary>
        /// 留言内容
        /// </summary>
        [Display(Name = "留言内容")]
        [Required]
        [StringLength(BXJGUtilsConsts.FeedbackContentMaxLength)]
        public required string Content { get; set; }
    }
}

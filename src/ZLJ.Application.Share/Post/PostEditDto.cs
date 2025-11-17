using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using ZLJ.Application.Common.Share.Roles;
using ZLJ.Application.Share.Roles;

namespace ZLJ.Application.Share.Post
{
    public class PostEditDto: RoleEditDto
    {
        /// <summary>
        /// 关注
        /// 可能用于多个地方，典型的是首页统计时是否显示此角色
        /// </summary>
        [DisplayName("是否关注")]
        public bool IsAttention { get; set; } = true;
    }
}
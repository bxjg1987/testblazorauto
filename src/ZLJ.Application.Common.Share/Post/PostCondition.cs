using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.Application.Common.Share.Roles;

namespace ZLJ.Application.Common.Share.Post
{
    public class PostCondition: RoleProviderCondition
    {   

        /// <summary>
         /// 关注
         /// 可能用于多个地方，典型的是首页统计时是否显示此角色
         /// </summary>
        public bool? IsAttention { get; set; }
    }
}

using BXJG.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.User
{
    /// <summary>
    /// 获取用户数据的条件模型
    /// </summary>
    public class UserGetAllCondition : IHaveKeywords
    {
        public string? Keywords { get; set; }
        public string? OuCode { get; set; }

        /// <summary>
        /// 区域编码
        /// </summary>
        public string? AreaCode { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string? Keyword { get; set; }
        /// <summary>
        /// 角色岗位id
        /// </summary>
        public int? PostId { get; set; }
    }
}

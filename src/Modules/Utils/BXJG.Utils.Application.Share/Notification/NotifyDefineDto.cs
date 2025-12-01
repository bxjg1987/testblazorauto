using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.Notification
{
    /// <summary>
    /// 通知定义
    /// </summary>
    public class NotifyDefineDto
    {
        /// <summary>
        /// 唯一名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 关联的实体类型全名
        /// </summary>
        public string EntityType { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get;set;}
        /// <summary>
        /// 附加属性
        /// </summary>
        public IDictionary<string, object> Attributes { get;  set; }

        //public bool Selected { get; set; }
        ///// <summary>
        ///// 未读数量
        ///// </summary>
        //public int UnReadCount { get; set; }
    }
}
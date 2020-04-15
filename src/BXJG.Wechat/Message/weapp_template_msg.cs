using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.WeChat.Message
{
    public class weapp_template_msg
    {
        /// <summary>
        /// 小程序模板ID
        /// </summary>
        public string template_id { get; set; }

        /// <summary>
        /// 小程序页面路径
        /// </summary>
        public string page { get; set; }

        /// <summary>
        /// 小程序模板消息formid
        /// </summary>

        public string form_id { get; set; }

        /// <summary>
        /// 小程序模板数据

        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 小程序模板放大关键词

        /// </summary>

        public string emphasis_keyword { get; set; }
    }
}

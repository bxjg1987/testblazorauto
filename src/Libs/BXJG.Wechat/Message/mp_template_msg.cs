using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.WeChat.Message
{
    public class mp_template_msg
    {
        /// <summary>
        /// 公众号appid，要求与小程序有绑定且同主体
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 公众号模板id
        /// </summary>
        public string template_id { get; set; }

        /// <summary>
        /// 公众号模板消息所要跳转的url
        /// </summary>

        public string url { get; set; }

        /// <summary>
        /// 公众号模板消息所要跳转的小程序，小程序的必须与公众号具有绑定关系
        /// </summary>
        public string miniprogram { get; set; }

        /// <summary>
        /// 公众号模板消息的数据
        /// </summary>

        public object data { get; set; }
    }
}

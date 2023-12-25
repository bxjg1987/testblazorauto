using BXJG.WeChat.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BXJG.WeChat.MiniProgram
{
    public class LoginResult : WechatResult
    {
        public string openid { get; set; }
        public string session_key { get; set; }
        public string unionid { get; set; }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace BXJG.WeChat.MiniProgram
{
    public class LoginContext 
    {
        public HttpContext Context { get; set; }
        public LoginResult Token { get; set; }
        public Option Option { get; set; }
    }
}

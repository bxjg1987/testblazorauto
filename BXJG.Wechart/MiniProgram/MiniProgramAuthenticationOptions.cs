using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace BXJG.Wechart.MiniProgram
{
    public class MiniProgramAuthenticationOptions: RemoteAuthenticationOptions
    {
        public MiniProgramAuthenticationOptions()
        {
            CallbackPath = new PathString(MiniProgramConsts.CallbackPath);
            UserInformationEndpoint = MiniProgramConsts.UserInformationEndpoint;
            BackchannelTimeout = TimeSpan.FromSeconds(60);

            //将微信返回的数据赋值到ticket的逻辑放到Handler中去处理了
            //ClaimActions.MapJsonKey("openid", "openid");
            //ClaimActions.MapJsonKey("session_key", "session_key");
            //ClaimActions.MapJsonKey("unionid", "unionid");
            //ClaimActions.MapCustomJson(ClaimTypes.Email, user => user.GetString("mail") ?? user.GetString("userPrincipalName"));
        }

        //public ClaimActionCollection ClaimActions { get; } = new ClaimActionCollection();

        public string UserInformationEndpoint { get; set; }

        public string AppId { get; set; }

        public string Secret { get; set; }

        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }
    }
}

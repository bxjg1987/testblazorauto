using Abp.Runtime.Session;
using BXJG.WeChat.Web.MiniProgram;

namespace ZLJ.Web.Host.Startup
{
    public class WxLoginHandler : ILoginHandler, ITransientDependency
    {
        IAbpSession abpSession;

        public WxLoginHandler(IAbpSession abpSession)
        {
            this.abpSession = abpSession;
        }

        public Task LoginAsync(LoginContext context)
        {
            var appKey = abpSession.GetAppKey();

            return Task.CompletedTask;
        }
    }
}

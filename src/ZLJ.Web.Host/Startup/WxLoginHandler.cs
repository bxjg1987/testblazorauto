using BXJG.WeChat.Web.MiniProgram;

namespace ZLJ.Web.Host.Startup
{
    public class WxLoginHandler : ILoginHandler, ITransientDependency
    {
        public Task LoginAsync(LoginContext context)
        {
            throw new NotImplementedException();
        }
    }
}

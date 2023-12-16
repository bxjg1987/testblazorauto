using Abp.MultiTenancy;
using Abp.Runtime.Session;

namespace ZLJ.Admin.CoreRCL.Startup
{
    /*
     * 服务端模式中使用的是abp的实现
     * 客户端模式都是去调用接口的，session是通过token在api那边识别出来的
     * 
     * 这里的实现，唯一目的是让组件的开发方式保持客户端和服务端一致
     */

    public class MyAbpSession : IAbpSession
    {
        //AuthenticationStateProvider _authenticationStateProvider;

        //public MyAbpSession(AuthenticationStateProvider authenticationStateProvider)
        //{
        //    _authenticationStateProvider = authenticationStateProvider;
        //}

        public long? UserId => 0;  //throw new NotImplementedException();

        public int? TenantId => 0;

        public MultiTenancySides MultiTenancySide => MultiTenancySides.Host;// throw new NotImplementedException();

        public long? ImpersonatorUserId => 0;// throw new NotImplementedException();

        public int? ImpersonatorTenantId => 0;// throw new NotImplementedException();
        class x : IDisposable
        {
            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
        public IDisposable Use(int? tenantId, long? userId)
        {
            return new x();
            //throw new NotImplementedException();
        }
    }
}

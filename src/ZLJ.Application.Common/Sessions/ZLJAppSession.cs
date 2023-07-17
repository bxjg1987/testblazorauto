using Abp.Configuration.Startup;
using Abp.MultiTenancy;
using Abp.Runtime;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.App.Common.Sessions
{
    public class ZLJAppSession : ClaimsAbpSession
    {
        //应用的某些功能可能不需要登陆，因此不能单纯使用claim
        //可以参考abp的租户解析器（一堆），所以它的租户解析器是反着来的，

        // public virtual string AppKey { get; set; }
      //  public IPrincipalAccessor PrincipalAcc => base.PrincipalAccessor;
        public virtual AppInfo AppInfo => appinfoProvider?.Invoke(base.PrincipalAccessor);
        Func<IPrincipalAccessor,AppInfo> appinfoProvider;
        public ZLJAppSession(IPrincipalAccessor principalAccessor, IMultiTenancyConfig multiTenancy, ITenantResolver tenantResolver, IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider, Func<IPrincipalAccessor, AppInfo> appinfoProvider) : base(principalAccessor, multiTenancy, tenantResolver, sessionOverrideScopeProvider)
        {
            this.appinfoProvider = appinfoProvider;
        }
    }
}
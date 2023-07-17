using Abp.Configuration.Startup;
using Abp.MultiTenancy;
using Abp.Runtime;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.App.Common.Sessions;

namespace ZLJ.App.Admin.Sessions
{
    public class AdminSession : ZLJAppSession, ISingletonDependency
    {
        public AdminSession(IPrincipalAccessor principalAccessor, IMultiTenancyConfig multiTenancy, ITenantResolver tenantResolver, IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider, Func<IPrincipalAccessor,AppInfo> appinfoProvider) : base(principalAccessor, multiTenancy, tenantResolver, sessionOverrideScopeProvider, appinfoProvider)
        {
        }
    }
}

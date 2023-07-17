using Abp.Configuration.Startup;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.App.Common.Sessions;

namespace ZLJ.App.Customer.Sessions
{
    public class CustomerSession : ZLJAppSession, ISingletonDependency
    {
        public CustomerSession(IPrincipalAccessor principalAccessor, IMultiTenancyConfig multiTenancy, ITenantResolver tenantResolver, IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider, Func<IPrincipalAccessor,AppInfo> appinfoProvider) : base(principalAccessor, multiTenancy, tenantResolver, sessionOverrideScopeProvider, appinfoProvider)
        {
        }
        public long? CustomerId
        {
            get
            {
                var userEmailClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "customerId");
                if (string.IsNullOrEmpty(userEmailClaim?.Value))
                {
                    return null;
                }

                return long.Parse(userEmailClaim.Value);
            }
        }
    }
}

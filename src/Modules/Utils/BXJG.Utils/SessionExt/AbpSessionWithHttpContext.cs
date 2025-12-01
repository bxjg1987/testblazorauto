using Abp.Configuration.Startup;
using Abp.MultiTenancy;
using Abp.Runtime;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Runtime.Session
{
    public class AbpSessionWithHttpContext : ClaimsAbpSession
    {
        protected internal readonly BXJG.Common.Session.ISession httpContextAccessor;
        public AbpSessionWithHttpContext(IPrincipalAccessor principalAccessor,
            IMultiTenancyConfig multiTenancy,
            ITenantResolver tenantResolver,
            IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider,
            BXJG.Common.Session.ISession httpContextAccessor) : base(principalAccessor, multiTenancy, tenantResolver, sessionOverrideScopeProvider)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        //public string GetAppKey() { 
        //    return httpContextAccessor.HttpContext.Request.GetAppKey();
        //}
    }
}

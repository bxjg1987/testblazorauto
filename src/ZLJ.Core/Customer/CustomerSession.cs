using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Customer
{
    //public class CustomerSession : ClaimsAbpSession, ITransientDependency
    //{
    //    public CustomerSession(
    //        IPrincipalAccessor principalAccessor,
    //        IMultiTenancyConfig multiTenancy,
    //        ITenantResolver tenantResolver,
    //        IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider) :
    //        base(principalAccessor, multiTenancy, tenantResolver, sessionOverrideScopeProvider)
    //    {

    //    }

    //    public long? CustomerId
    //    {
    //        get
    //        {
    //            var userEmailClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "customerId");
    //            if (string.IsNullOrEmpty(userEmailClaim?.Value))
    //            {
    //                return null;
    //            }

    //            return long.Parse( userEmailClaim.Value);
    //        }
    //    }
    //}
}

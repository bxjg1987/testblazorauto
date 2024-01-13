using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.Authorization.Permissions
{
    public class CommonAppAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
           // throw new NotImplementedException();
        }
    }
}

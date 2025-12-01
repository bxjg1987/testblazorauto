using Abp.Authorization;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Auth
{
    public class AuthenticatePermissionDependency : IPermissionDependency//,ISingletonDependency
    {
        public static readonly AuthenticatePermissionDependency Instance = new AuthenticatePermissionDependency();
        public bool IsSatisfied(IPermissionDependencyContext context)
        {
            return context.User!=default;
        }

        public Task<bool> IsSatisfiedAsync(IPermissionDependencyContext context)
        {
            return Task.FromResult(context.User != default);
        }
    }
}

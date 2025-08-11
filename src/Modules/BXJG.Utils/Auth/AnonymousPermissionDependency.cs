using Abp.Authorization;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Auth
{
    public class AnonymousPermissionDependency : IPermissionDependency//, ISingletonDependency
    {
        public static readonly AnonymousPermissionDependency Instance = new AnonymousPermissionDependency();
        public bool IsSatisfied(IPermissionDependencyContext context)
        {
            return true;
        }

        public Task<bool> IsSatisfiedAsync(IPermissionDependencyContext context)
        {
            return Task.FromResult( true);
        }
    }
}

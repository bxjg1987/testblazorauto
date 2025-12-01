using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BXJG.Utils.BusinessUser
{
    /*
     * https://aspnetboilerplate.com/Pages/Documents/Articles%5CHow-To%5Cadd-custom-session-field-aspnet-core
     * 相比abp官方文档方式，我们抽象了个接口出来
     * 
     * 我们的文档：https://gitee.com/bxjg1987_admin/abp/wikis/多种用户类型?sort_id=3639713
     */
    [Obsolete("改为继承的方式了")]
    public interface IBusinessUserSession<TKey>
    {
        //没必要用task，因为业务用户id必须快速获取
        TKey BusinessUserId { get; }
    }
    [Obsolete("改为继承的方式了")]
    public class BusinessUserClaimSession<TKey> : ClaimsAbpSession, IBusinessUserSession<TKey>
    {
        protected readonly string businessUserClaimType;

        public BusinessUserClaimSession(IPrincipalAccessor principalAccessor,
                                        IMultiTenancyConfig multiTenancy,
                                        ITenantResolver tenantResolver,
                                        IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider,
                                        string businessUserClaimType) : base(principalAccessor,
                                                                             multiTenancy,
                                                                             tenantResolver,
                                                                             sessionOverrideScopeProvider)
        {
            this.businessUserClaimType = businessUserClaimType;
        }

        public virtual TKey BusinessUserId
        {
            get
            {
                var userEmailClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == businessUserClaimType);
                if (string.IsNullOrEmpty(userEmailClaim?.Value))
                    return default;

                //return userEmailClaim.Value.ToNullable<TKey>();
                // return (TKey)Convert.ChangeType(userEmailClaim.Value, typeof(TKey));
                var t = typeof(TKey);

                if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    if (userEmailClaim.Value == null)
                    {
                        return default(TKey);
                    }

                    t = Nullable.GetUnderlyingType(t);
                }

                return (TKey)Convert.ChangeType(userEmailClaim.Value, t);
            }
        }

       
    }
}

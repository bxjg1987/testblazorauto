using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Runtime.Session;
using ZLJ.App.Common.Sessions.Dto;

namespace ZLJ.App.Common.Sessions
{
    /// <summary>
    /// 登陆时获取全局应用程序信息和当前登陆用户信息
    /// 不同app可以直接使用此接口，也可以提供继承实现特定的功能
    /// </summary>
    public class SessionAppService : CommonBaseAppService
    {
        [DisableAuditing]
        public virtual async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var ainfo = Assembly.GetExecutingAssembly();
            var appVersion = ainfo.GetName().Version;
            var abpVersion = typeof(Abp.AbpKernelModule).Assembly.GetName().Version;

            var output = await Create();

             output.Application = new ApplicationInfoDto
            {
                Version = appVersion.Major + "." + appVersion.Minor,
                ReleaseDate = File.GetLastWriteTime(ainfo.Location),
                Features = new Dictionary<string, bool>(),
                RunTimeVersion = Environment.Version.ToString(),
                AbpVersion = abpVersion.Major + "." + abpVersion.Minor
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }
           // base.Logger.Debug($"获取当前应用信息中的用户id：{AbpSession.UserId}");
           
            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
            }

            return output;
        }

        protected virtual ValueTask< GetCurrentLoginInformationsOutput> Create()
        {
            return ValueTask.FromResult( new GetCurrentLoginInformationsOutput());
        }
    }
}

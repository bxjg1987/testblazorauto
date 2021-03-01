using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Auditing;
using ZLJ.Sessions.Dto;

namespace ZLJ.Sessions
{
    public class SessionAppService : ZLJAppServiceBase, ISessionAppService
    {
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var ainfo = Assembly.GetExecutingAssembly();
            var appVersion = ainfo.GetName().Version;
            var abpVersion = typeof(Abp.AbpKernelModule).Assembly.GetName().Version;

            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = appVersion.Major + "." + appVersion.Minor,
                    ReleaseDate = File.GetLastWriteTime(ainfo.Location),
                    Features = new Dictionary<string, bool>(),
                    RunTimeVersion = Environment.Version.ToString(),
                    AbpVersion = abpVersion.Major + "." + abpVersion.Minor
                }
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }

            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
            }

            return output;
        }
    }
}

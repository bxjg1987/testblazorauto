using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Auditing;
using Abp.Runtime.Session;

namespace BXJG.Utils.Application.Share.Session
{
    /// <summary>
    /// 登陆时获取全局应用程序信息和当前登陆用户信息
    /// 不同app可以直接使用此接口，也可以提供继承实现特定的功能
    /// </summary>
    public interface ISessionAppService : IApplicationService
    {
        [DisableAuditing]
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        //protected virtual ValueTask< GetCurrentLoginInformationsOutput> Create()
        //{
        //    return ValueTask.FromResult( new GetCurrentLoginInformationsOutput());
        //}
    }
}

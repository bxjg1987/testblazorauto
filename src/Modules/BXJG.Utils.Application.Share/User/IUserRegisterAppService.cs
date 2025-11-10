using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.User
{


    /// <summary>
    /// 用户注册服务接口
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public interface IUserRegisterAppService<TInput,TOutput>:IApplicationService
    {
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
         Task<TOutput> RegisterAsync(TInput input);
    }
}

using Abp.Domain.Entities;
using Abp.Localization.Sources;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.UI
{
    /// <summary>
    /// 抛出一个abp的用户友好异常，可以使用也可以不使用本地化源
    /// </summary>
    public static class UserFriendlyExceptionFactory
    {
        /// <summary>
        /// 抛出一个UserFriendlyException。
        /// 可选地使用本地化源，此时name为本地化键
        /// </summary>
        /// <param name="nameOrMsg"></param>
        /// <param name="localizationSource"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public static void Throw(string nameOrMsg, ILocalizationSource? localizationSource = default, params IEnumerable<object> args)
        {
            throw GetException(nameOrMsg, localizationSource, args);
        }
        /// <summary>
        /// 返回一个UserFriendlyException。
        /// 可选地使用本地化源，此时name为本地化键
        /// </summary>
        /// <param name="nameOrMsg">本地化键或具体的消息</param>
        /// <param name="localizationSource"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static UserFriendlyException GetException(string nameOrMsg, ILocalizationSource? localizationSource = default, params IEnumerable<object> args)
        {
            //if(name.IsNullOrWhiteSpaceBXJG())
            //    throw new UserFriendlyException("异常信息不能为空");

            if (localizationSource != null)
                return new UserFriendlyException(localizationSource.GetString(nameOrMsg, args));

            if (args != null && args.Any())
                return new UserFriendlyException(string.Format(nameOrMsg, args));

            return new UserFriendlyException(nameOrMsg);
        }
    }
}

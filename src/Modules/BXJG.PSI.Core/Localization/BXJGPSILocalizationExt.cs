using Abp.Localization;
using Abp.Localization.Sources;
using BXJG.PSI;
using BXJG.Utils.Share;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Localization
{
    public static class BXJGPSILocalizationExt
    {
       
        /// <summary>
        /// 获取通用模块中的本地化字符串
        /// </summary>
        /// <param name="localizationManager"></param>
        /// <param name="name"></param>
        /// <param name="args">注意不要传递数组</param>
        /// <returns></returns>
        public static string PSILString(this ILocalizationManager localizationManager, string name, params object[] args)
        {
            return string.Format(localizationManager.GetString(BXJGPSICoreConsts.LocalizationSourceName, name), args);
        }
    }
}
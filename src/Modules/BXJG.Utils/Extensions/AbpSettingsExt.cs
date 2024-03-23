using BXJG.Utils.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Configuration
{
    public static class AbpSettingsExt
    {
        /// <summary>
        /// 获取上传根目录
        /// </summary>
        /// <param name="settingManager"></param>
        /// <returns></returns>
        public static Task<string> HuoquShangchuangen(this ISettingManager settingManager)
        {
            return settingManager.GetSettingValueForApplicationAsync(BXJGUtilsConsts.Shangchuangen);
        }
    }
}

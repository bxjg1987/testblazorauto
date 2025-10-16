using Abp.Configuration;
using BXJG.Utils.Localization;
using BXJG.Utils.Share;
using BXJG.Utils.Share.Files;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BXJG.Utils.Settings
{
    /// <summary>
    /// 这里只是通用设置，各模块可以有自己的设置
    /// </summary>
    public class BXJGUtilsSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            var sys = new SettingDefinitionGroup(BXJGUtilsConsts.QuanjuPeizhi, BXJGUtilsConsts.QuanjuPeizhi.UtilsLI());

            var file = new SettingDefinitionGroup(BXJGUtilsConsts.SettingKeyUploadGroup, BXJGUtilsConsts.SettingKeyUploadGroup.UtilsLI());

            //默认值 常量 不应该定义在 接口层

            var yzm = new SettingDefinitionGroup(BXJGUtilsConsts.SettingKeyCaptchaGroup, BXJGUtilsConsts.SettingKeyCaptchaGroup.UtilsLI());

            return [
                ////服务器根
                //new SettingDefinition(BXJGUtilsConsts.FuwuqiGen,
                //                      "http://127.0.0.1:5000/",
                //                      BXJGUtilsConsts.FuwuqiGen.UtilsLI(),
                //                      sys,
                //                      scopes: SettingScopes.Application ,
                //                      isVisibleToClients:true),
                ////上传根
                //new SettingDefinition(BXJGUtilsConsts.Shangchuangen,
                //                      RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "D:\\bxjg_uploads\\":"/var/bxjg_uploads",
                //                      BXJGUtilsConsts.Shangchuangen.UtilsLI(),
                //                      sys,
                //                      scopes: SettingScopes.Application ,
                //                      isVisibleToClients:true),
                //允许的文件类型
                new SettingDefinition(BXJGUtilsConsts.SettingKeyUploadType,
                                      ".txt,.jpg,.jpeg,.gif,.png,.doc,.docx,.rar,.xlsx,.xls,.pdf",
                                      BXJGUtilsConsts.SettingKeyUploadType.UtilsLI(),
                                      file,
                                      scopes: SettingScopes.Application ,
                                      isVisibleToClients:true),

                //单个文件允许的大小(mb)
                new SettingDefinition(BXJGUtilsConsts.SettingKeyUploadSize,
                                      (1024*50).ToString(),
                                      BXJGUtilsConsts.SettingKeyUploadSize.UtilsLI(),
                                      file,
                                      scopes: SettingScopes.Application | SettingScopes.Tenant,
                                      isVisibleToClients:true),

                new SettingDefinition(BXJGUtilsConsts.CaptchaOptions_ImageOption_FontFamily,
                                      "Kaiti",
                                      BXJGUtilsConsts.CaptchaOptions_ImageOption_FontFamily.UtilsLI(),
                                      yzm,
                                      scopes: SettingScopes.Application,
                                      isVisibleToClients:false),
                new SettingDefinition(BXJGUtilsConsts.CaptchaOptions_IgnoreCase,
                                      "true",
                                      BXJGUtilsConsts.CaptchaOptions_IgnoreCase.UtilsLI(),
                                      yzm,
                                      scopes: SettingScopes.Application,
                                      isVisibleToClients:false),
                new SettingDefinition(BXJGUtilsConsts.CaptchaOptions_CaptchaType,
                                      "5",
                                      BXJGUtilsConsts.CaptchaOptions_CaptchaType.UtilsLI(),
                                      yzm,
                                      scopes: SettingScopes.Application,
                                      isVisibleToClients:false),
                new SettingDefinition(BXJGUtilsConsts.CaptchaOptions_ImageOption_Animation,
                                      "false",
                                      BXJGUtilsConsts.CaptchaOptions_ImageOption_Animation.UtilsLI(),
                                      yzm,
                                      scopes: SettingScopes.Application,
                                      isVisibleToClients:false)
            ];
        }
    }
}

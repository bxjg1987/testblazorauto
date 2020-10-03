using Abp.Configuration;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.File
{
    /// <summary>
    /// 这里只是通用设置，各模块可以有自己的设置
    /// </summary>
    public class BXJGUtilsFileSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            var sys = new SettingDefinitionGroup(Consts.SettingKeyUploadGroup, "文件上传设置".UtilsLI());

            return new[]
            {
                new SettingDefinition(
                    Consts.SettingKeyUploadType,
                    Consts.DefaultUploadTypes,
                    "允许的文件类型".UtilsLI(),
                    sys,
                    scopes: SettingScopes.Application ,
                    isVisibleToClients:false),

                 new SettingDefinition(
                    Consts.SettingKeyUploadSize,
                    Consts.DefaultUploadMaxSize.ToString(),
                    "允许的大小(Kb)".UtilsLI(),
                    sys,
                    scopes: SettingScopes.Application ,
                    isVisibleToClients:false)
            };
        }
    }
}

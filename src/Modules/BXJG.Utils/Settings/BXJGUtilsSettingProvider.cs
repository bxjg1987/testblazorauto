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
                                      isVisibleToClients:true,
                                           customData: new
                                           {
                                               csharpType = "string",//typeof(c#类型).Name //inputType="checkbox",//text,select 等等
                                               isRequired = false,//此配置是否必填
                                               formatter = string.Empty,
                                               min = 0,//最小值
                                               max = 0,//最大值
                                               placeholder = "留空则仅允许常见文件类型",//占位符
                                                                         //在前端管理设置定义时会序列化为Dictionary<string,string>，所以这里只能使用字符串作为可选值，前端再反序列化一次
                                                                         //key的类型与csharpType对应
                                                                         //配合默认值
                                                                         //若多选，则用 a,b,c方式存储
                                                                         //selectValues = System.Text.Json.JsonSerializer.Serialize(new Dictionary<string, string>{
                                                                         //    { "a","aaa"},
                                                                         //    { "b","bbb"},
                                                                         //    { "c","ccc"},
                                                                         //}),
                                                                         //是否可以多选
                                                                         //isMultiSelect = false,
                                           }),

                //单个文件允许的大小(mb)
                new SettingDefinition(BXJGUtilsConsts.SettingKeyUploadSize,
                                      (1024*50).ToString(),
                                      BXJGUtilsConsts.SettingKeyUploadSize.UtilsLI(),
                                      file,
                                      scopes: SettingScopes.Application | SettingScopes.Tenant,
                                      isVisibleToClients:true,
                                           customData: new
                                           {
                                               csharpType = "int",//typeof(c#类型).Name //inputType="checkbox",//text,select 等等
                                               isRequired = true,//此配置是否必填
                                               formatter = string.Empty,
                                               min = 0,//最小值
                                               max = 0,//最大值
                                               placeholder = "单位mb",//占位符
                                                                         //在前端管理设置定义时会序列化为Dictionary<string,string>，所以这里只能使用字符串作为可选值，前端再反序列化一次
                                                                         //key的类型与csharpType对应
                                                                         //配合默认值
                                                                         //若多选，则用 a,b,c方式存储
                                                                         //selectValues = System.Text.Json.JsonSerializer.Serialize(new Dictionary<string, string>{
                                                                         //    { "a","aaa"},
                                                                         //    { "b","bbb"},
                                                                         //    { "c","ccc"},
                                                                         //}),
                                                                         //是否可以多选
                                                                         //isMultiSelect = false,
                                           }),

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
                                      isVisibleToClients:false,
                                           customData: new
                                           {
                                               csharpType = "bool",//typeof(c#类型).Name //inputType="checkbox",//text,select 等等
                                               isRequired = true,//此配置是否必填
                                               formatter = string.Empty,
                                               min = 0,//最小值
                                               max = 0,//最大值
                                               placeholder = "忽略大小写",//占位符
                                                                         //在前端管理设置定义时会序列化为Dictionary<string,string>，所以这里只能使用字符串作为可选值，前端再反序列化一次
                                                                         //key的类型与csharpType对应
                                                                         //配合默认值
                                                                         //若多选，则用 a,b,c方式存储
                                                                         //selectValues = System.Text.Json.JsonSerializer.Serialize(new Dictionary<string, string>{
                                                                         //    { "a","aaa"},
                                                                         //    { "b","bbb"},
                                                                         //    { "c","ccc"},
                                                                         //}),
                                                                         //是否可以多选
                                                                         //isMultiSelect = false,
                                           }),
                new SettingDefinition(BXJGUtilsConsts.CaptchaOptions_CaptchaType,
                                      "5",
                                      BXJGUtilsConsts.CaptchaOptions_CaptchaType.UtilsLI(),
                                      yzm,
                                      scopes: SettingScopes.Application,
                                      isVisibleToClients:false,
                                           customData: new
                                           {
                                               csharpType = "int",//typeof(c#类型).Name //inputType="checkbox",//text,select 等等
                                               isRequired = true,//此配置是否必填
                                               formatter = string.Empty,
                                               min = 0,//最小值
                                               max = 0,//最大值
                                               placeholder = "",//占位符
                                                                         //在前端管理设置定义时会序列化为Dictionary<string,string>，所以这里只能使用字符串作为可选值，前端再反序列化一次
                                                                         //key的类型与csharpType对应
                                                                         //配合默认值
                                                                         //若多选，则用 a,b,c方式存储
                                                                         //selectValues = System.Text.Json.JsonSerializer.Serialize(new Dictionary<string, string>{
                                                                         //    { "a","aaa"},
                                                                         //    { "b","bbb"},
                                                                         //    { "c","ccc"},
                                                                         //}),
                                                                         //是否可以多选
                                                                         //isMultiSelect = false,
                                           }),
                new SettingDefinition(BXJGUtilsConsts.CaptchaOptions_ImageOption_Animation,
                                      "false",
                                      BXJGUtilsConsts.CaptchaOptions_ImageOption_Animation.UtilsLI(),
                                      yzm,
                                      scopes: SettingScopes.Application,
                                      isVisibleToClients:false,
                                           customData: new
                                           {
                                               csharpType = "bool",//typeof(c#类型).Name //inputType="checkbox",//text,select 等等
                                               isRequired = true,//此配置是否必填
                                               formatter = string.Empty,
                                               min = 0,//最小值
                                               max = 0,//最大值
                                               placeholder = "",//占位符
                                                                         //在前端管理设置定义时会序列化为Dictionary<string,string>，所以这里只能使用字符串作为可选值，前端再反序列化一次
                                                                         //key的类型与csharpType对应
                                                                         //配合默认值
                                                                         //若多选，则用 a,b,c方式存储
                                                                         //selectValues = System.Text.Json.JsonSerializer.Serialize(new Dictionary<string, string>{
                                                                         //    { "a","aaa"},
                                                                         //    { "b","bbb"},
                                                                         //    { "c","ccc"},
                                                                         //}),
                                                                         //是否可以多选
                                                                         //isMultiSelect = false,
                                           })
            ];
        }
    }
}

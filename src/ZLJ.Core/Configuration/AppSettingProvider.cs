using System.Collections.Generic;
using System.Linq;
using Abp.Configuration;
using BXJG.Utils.Localization;
using BXJG.Utils.Share;
using BXJG.Utils.Share.Files;
using Microsoft.Extensions.Configuration;
using ZLJ.Core.Localization;
//using ZLJ.WorkOrder.Workload;

namespace ZLJ.Core.Configuration
{
    //有些领域服务需要，所以就定义在这里吧
    //其实定义在ZLJ.Application.Common中也可以，不过配置key的常量必须在ZLJ.Core.Share

    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            var sys = new SettingDefinitionGroup("sys", "系统".UtilsLI());
            var sys2 = new SettingDefinitionGroup(BXJGUtilsConsts.SettingKeyUploadGroup, "文件上传设置".UtilsLI());
            //  var shangchuang = new SettingDefinitionGroup(ZLJ.Core.Share.ZLJConsts.CfgKeyUpload, "文件上传设置".UtilsLI());
            var list = new List<SettingDefinition>
            {
                new SettingDefinition(ZLJ.Core.Share.ZLJConsts.AppName,
                "BXJGABP",
                ZLJ.Core.Share.ZLJConsts.AppName.UtilsLI(),
                group:sys,
                scopes: SettingScopes.Application ,
                isVisibleToClients: true),
                new SettingDefinition(ZLJ.Core.Share.ZLJConsts.CfgKeyUpload,"D:\\bxjg_upload_files\\",
                                      "存储路径".GetLocalizableString(),
                                      sys2,
                                      scopes: SettingScopes.Application,
                                      isVisibleToClients: false,
                                      isEncrypted:true,
                                      customData:new {
                                          csharpType = "string",//typeof(c#类型).Name //inputType="checkbox",//text,select 等等
                                          isRequired=true,//此配置是否必填
                                          formatter = "",
                                          min=0,//最小值
                                          max=0,//最大值
                                          //placeholder="请输入数字1",//占位符 可以字节用描述字段
                                          //在前端管理设置定义时会序列化为Dictionary<string,string>，所以这里只能使用字符串作为可选值，前端再反序列化一次
                                          //key的类型与csharpType对应
                                          //配合默认值
                                          //若多选，则用 a,b,c方式存储
                                          selectValues= System.Text.Json.JsonSerializer.Serialize( new Dictionary<string,string>{
                                              { "a","aaa"},
                                              { "b","bbb"},
                                              { "c","ccc"},
                                          }),
                                        //是否可以多选
                                      isMultiSelect=false}),

                //new SettingDefinition(
                //    BXJGUtilsConsts.SettingKeyUploadType,
                //    BXJGUtilsConsts.DefaultUploadTypes + ",docx",
                //    "允许的文件类型".UtilsLI(),
                //    sys2,
                //    scopes: SettingScopes.Application,
                //    isVisibleToClients: false),
                new SettingDefinition(AppSettingNames.UiTheme,
                                      "red",
                                      "风格".GetLocalizableString(),
                                      sys2,
                                      scopes: SettingScopes.All,
                                      isVisibleToClients: true)
            };
            #region 全局速率限制
            var slxz = new SettingDefinitionGroup("lptslxz", "全局令牌桶速率限制".UtilsLI());
            list.Add(new SettingDefinition(ZLJ.Core.Share.ZLJConsts.TokenLimit,
                "120",
                ZLJ.Core.Share.ZLJConsts.TokenLimit.UtilsLI(),
                group: slxz,
                scopes: SettingScopes.Application,
                isVisibleToClients: false));
            list.Add(new SettingDefinition(ZLJ.Core.Share.ZLJConsts.TokensPerPeriod,
                "8",
                ZLJ.Core.Share.ZLJConsts.TokensPerPeriod.UtilsLI(),
                group: slxz,
                scopes: SettingScopes.Application,
                isVisibleToClients: false));
            list.Add(new SettingDefinition(ZLJ.Core.Share.ZLJConsts.ReplenishmentPeriod,
                "10",
                ZLJ.Core.Share.ZLJConsts.ReplenishmentPeriod.UtilsLI(),
                group: slxz,
                scopes: SettingScopes.Application,
                isVisibleToClients: false));
            #endregion

            return list;
            // return list.Union(GetTenantSettings());
        }

        //private IEnumerable<SettingDefinition> GetTenantSettings()
        //{
        //    return new[]
        //    {
        //        new SettingDefinition(AppSettingNames.TenantManagement.Workload.WorkloadType,
        //            WorkloadType.ByPoints.ToString(),
        //            scopes: SettingScopes.Tenant, isVisibleToClients: true),
        //        new SettingDefinition(AppSettingNames.TenantManagement.Workload.WorkloadRuleType,
        //            WorkloadRuleType.ByWorkYears.ToString(),
        //            scopes: SettingScopes.Tenant, isVisibleToClients: true),
        //        new SettingDefinition(AppSettingNames.TenantManagement.Workload.WorkloadPoints,
        //            "200",
        //            scopes: SettingScopes.Tenant, isVisibleToClients: true)
        //    };
        //}

    }
}

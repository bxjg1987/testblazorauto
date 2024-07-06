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
            var sys = new SettingDefinitionGroup(BXJGUtilsConsts.SettingKeyUploadGroup, "文件上传设置".UtilsLI());
          //  var shangchuang = new SettingDefinitionGroup(ZLJ.Core.Share.ZLJConsts.CfgKeyUpload, "文件上传设置".UtilsLI());



            var sys2 = new SettingDefinitionGroup(
                   ZLJ.Core.Share.ZLJConsts.DataDictionaryMigrationValueSettingGroupKey,
                   "数据字典下拉值".GetLocalizableString());
            var list=new[]
            {
                new SettingDefinition(
                    ZLJ.Core.Share.ZLJConsts.DataDictionaryMigrationValuePrinterBrand,
                    "0",
                    "设备品牌数据字典Id".GetLocalizableString(),
                    sys2,
                    scopes: SettingScopes.Tenant,
                    isVisibleToClients: true),

                new SettingDefinition(
                    ZLJ.Core.Share.ZLJConsts.DataDictionaryMigrationValueCustomerCategory,
                    "0",
                    "客户类别数据字典Id".GetLocalizableString(),
                    sys2,
                    scopes: SettingScopes.Tenant,
                    isVisibleToClients: true),
                new SettingDefinition(
                    ZLJ.Core.Share.ZLJConsts.DataDictionaryMigrationValueCustomerLevel,
                    "0",
                    "客户级别数据字典Id".GetLocalizableString(),
                    sys2,
                    scopes: SettingScopes.Tenant,
                    isVisibleToClients: true),
                new SettingDefinition(
                    ZLJ.Core.Share.ZLJConsts.DataDictionaryMigrationValuePost,
                    "0",
                    "岗位字典Id".GetLocalizableString(),
                    sys2,
                    scopes: SettingScopes.Tenant,
                    isVisibleToClients: true),

                 new SettingDefinition(
                    ZLJ.Core.Share.ZLJConsts.CfgKeyUpload,
                    "D:\\bxjg_upload_files\\",
                    "存储路径".GetLocalizableString(),
                    sys,
                    scopes: SettingScopes.Application,
                    isVisibleToClients: true),

                //new SettingDefinition(
                //    BXJGUtilsConsts.SettingKeyUploadType,
                //    BXJGUtilsConsts.DefaultUploadTypes + ",docx",
                //    "允许的文件类型".UtilsLI(),
                //    sys2,
                //    scopes: SettingScopes.Application,
                //    isVisibleToClients: false),
                new SettingDefinition(AppSettingNames.UiTheme, "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
            };

           

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

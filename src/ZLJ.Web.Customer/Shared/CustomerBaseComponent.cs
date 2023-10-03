using Abp.Localization.Sources;
using Abp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudBlazor;
using Microsoft.Extensions.DependencyInjection;
using BXJG.Utils;
using BXJG.AbpMudBlazor.Components;

namespace ZLJ.Web.Customer.Shared
{
    /// <summary>
    /// 后台管理端的blazor组件抽象类
    /// </summary>
    public class CustomerBaseComponent : AbpMudBaseComponent
    {
        private ILocalizationSource appCommonLocalizationSource, zljLocalizationSource, utilsLocalizationSource;

        protected virtual ILocalizationSource LocalizationSourceAppCommon
        {
            get
            {
                if (appCommonLocalizationSource == null || appCommonLocalizationSource.Name != ZLJ.App.Common.Consts.Common)
                {
                    appCommonLocalizationSource = LocalizationManager.GetSource(ZLJ.App.Common.Consts.Common);
                }

                return appCommonLocalizationSource;
            }
        }
        protected virtual ILocalizationSource LocalizationSourceAppZLJ
        {
            get
            {

                if (zljLocalizationSource == null || zljLocalizationSource.Name != ZLJConsts.LocalizationSourceName)
                {
                    zljLocalizationSource = LocalizationManager.GetSource(ZLJConsts.LocalizationSourceName);
                }

                return zljLocalizationSource;
            }
        }
        protected virtual ILocalizationSource LocalizationSourceUtils
        {
            get
            {

                if (utilsLocalizationSource == null || utilsLocalizationSource.Name != BXJGUtilsConsts.LocalizationSourceName)
                {
                    utilsLocalizationSource = LocalizationManager.GetSource(BXJGUtilsConsts.LocalizationSourceName);
                }

                return utilsLocalizationSource;
            }
        }

        protected override string LocalizationSourceName => ZLJ.App.Customer.CustConsts.Cust;

     

        //public override void ShowError(string msg)
        //{
        //    Snackbar.Add(msg, Severity.Error);
        //}

        //public override ValueTask ShowErrorAsync(string msg)
        //{
        //    Snackbar.Add(msg, Severity.Error);
        //    return ValueTask.CompletedTask;
        //}
    }
}
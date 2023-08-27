using Abp.Localization.Sources;
using Abp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudBlazor;
using Microsoft.Extensions.DependencyInjection;
using ZLJ.App.Customer;
using BXJG.Utils;

namespace ZLJ.Web.Customer.Views.Shared
{
    /// <summary>
    /// 后台管理端的blazor组件抽象类
    /// </summary>
    public class CustomerBaseComponent : AbpBaseComponent
    {
        private Lazy<ISnackbar> _snackbar;
        protected IDialogService DialogService { get; private set; }
        protected ISnackbar Snackbar => _snackbar.Value;
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.DialogService = base.ScopedServices.GetRequiredService<IDialogService>();
            this._snackbar = base.ScopedServices.GetRequiredService<Lazy<ISnackbar>>();
        }

        //
        // 摘要:
        //     Gets/sets name of the localization source that is used in this application service.
        //     It must be set in order to use Abp.AbpServiceBase.L(System.String) and Abp.AbpServiceBase.L(System.String,System.Globalization.CultureInfo)
        //     methods.
        // protected string LocalizationSourceNameAdmin { get; set; } = AdminConsts.Admin;
        private ILocalizationSource _localizationSourceAdmin;
        //
        // 摘要:
        //     Gets localization source. It's valid if Abp.AbpServiceBase.LocalizationSourceName
        //     is set.
        protected ILocalizationSource LocalizationSourceCustomer
        {
            get
            {

                if (_localizationSourceAdmin == null || _localizationSourceAdmin.Name != CustConsts.Cust)
                {
                    _localizationSourceAdmin = LocalizationManager.GetSource(CustConsts.Cust);
                }

                return _localizationSourceAdmin;
            }
        }

        /// <summary>
        /// 显示成功的消息，木有考虑本地化
        /// </summary>
        /// <param name="opt"></param>
        /// <param name="msg"></param>
        public virtual void ShowSuccess(string opt = "操作", string msg = "{0}成功！")
        {
            Snackbar.Add(string.Format(msg, opt), Severity.Success);
        }

        public override void ShowError(string msg)
        {
            Snackbar.Add(msg, Severity.Error);
        }

        public override ValueTask ShowErrorAsync(string msg)
        {
            Snackbar.Add(msg, Severity.Error);
            return ValueTask.CompletedTask;
        }
    }
}

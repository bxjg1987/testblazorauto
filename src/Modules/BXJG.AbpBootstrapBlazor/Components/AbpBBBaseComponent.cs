using Abp;
using Abp.Application.Features;
using Abp.AspNetCore.Configuration;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.MultiTenancy;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.UI;
using BXJG.AbpBootstrapBlazor.Interceptors;
using BXJG.Common;
using BXJG.Common.Dto;
using BXJG.Common.RCL;
using BXJG.Utils.Components;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace BXJG.AbpBootstrapBlazor.Components
{
    /// <summary>
    /// 与Abp和BB有关的抽象组件（注意它与crud抽象组件是平级的）
    /// </summary>
    public abstract class AbpBBBaseComponent/*<TUser, TUserManager, TRole>*/ : AbpBaseComponent// OwningComponentBase
    //where TUser : AbpUser<TUser>
    //where TRole : AbpRole<TUser>, new()
    //where TUserManager : AbpUserManager<TRole, TUser>
    {
        [Inject]
        public MessageService MessageService { get; set; }
        protected override async ValueTask ShowFailMessage(string title = "操作提示", string msg = "操作失败！")
        {
            await MessageService.Show(new MessageOption()
            {
                Content = msg,
                Color = Color.Danger,
                ShowShadow = true,
                ShowBorder = true,
            });
        }
        protected override async ValueTask ShowSuccessMessage(string title = "操作提示", string msg = "操作成功！")
        {
            await MessageService.Show(new MessageOption()
            {
                Content = msg,
                Color = Color.Success,
                ShowShadow = true,
                ShowBorder = true
            });
        }
        #region 生命周期方法增加统一异常处理拦截器
        [AbpBBException]
        public override async Task SetParametersAsync(ParameterView parameters)
        {
           await base.SetParametersAsync(parameters);
        }
        [AbpBBException]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
        [AbpBBException]
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }
        [AbpBBException]
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        [AbpBBException]
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
        [AbpBBException]
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }
        [AbpBBException]
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return base.OnAfterRenderAsync(firstRender);
        }
        #endregion
    }
}

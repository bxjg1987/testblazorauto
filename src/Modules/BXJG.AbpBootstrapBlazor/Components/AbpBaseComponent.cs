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
using BXJG.Common;
using BXJG.Common.Dto;
using BXJG.Common.RCL;
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
    /*
     * 基本上把抽象的应用服务中的abp相关玩意搞过来
     * 注意只有scope tran服务才需要特殊处理？ abp是做了这个处理的，但blazor框架如果够聪明的话，单例对象它应该自己处理
     * 
     * UOW分析
     * 组件调用应用服务，应用服务的uow范围应该是不依赖外部的，而是通过proxy自动实现的
     * 所以组件中通常是不需要开启uow的，所以不需要做任何uow相关的封装，此时一个应用服务的一个方法对应一个uow
     * 如果有必要，可以在组件中开启uow，这样多个应用服务通过环境uow共享要给组件中的uow范围
     * 
     * 也不可能在所有事件处理程序中去自动开启uow，可能很多操作仅仅是界面交互，不需要uow
     * 
     * 既然blazor组件是注册到ioc的，那么使用动态代理应该也可以做aop，没测试过
     * 如果可行，可以定义Attribute，用户在指定方法上应用Attribute即可自动处理uow
     * 目前不考虑，以后再说吧
     * 
     * 总的来说，通常是不需要的，应用用例基本都是对应到应用服务的
     */

    /// <summary>
    /// 与abp关联的抽象组件
    /// 它使用延迟引用了abp常用接口
    /// </summary>
    public abstract class AbpBaseComponent/*<TUser, TUserManager, TRole>*/ : AbpBaseComponent// OwningComponentBase
    //where TUser : AbpUser<TUser>
    //where TRole : AbpRole<TUser>, new()
    //where TUserManager : AbpUserManager<TRole, TUser>
    {
        
    }

}

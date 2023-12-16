using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common.RCL
{
    /// <summary>
    /// 包含通用功能的抽象组件，与abp和具体项目无关的
    /// </summary>
    public abstract class CommonBaseComponent : OwningComponentBase
    {
        //// private Zhongjie zhongjieGlobal;
        // /// <summary>
        // /// 获取变形精怪中介，整个应用全局的
        // /// </summary>
        // protected virtual Zhongjie GlobalZhongjie => Context.GlobalZhongjie;// zhongjieGlobal ??= ScopedServices.GetRequiredService<Zhongjie>();
        // /// <summary>
        // /// 级联的事件总线，外层组件所处的层次决定此事件总线的范围
        // /// </summary>
        // protected virtual Zhongjie? Zhongjie => Context.Zhongjie;

        [Inject]
        protected virtual Zhongjie GlobalZhongjie { get; private set; }

        //[CascadingParameter]
        //protected BlazorServerContext Context { get; set; }

        private ILogger _logger;
        /// <summary>
        /// 日志
        /// </summary>
        protected virtual ILogger MicrosoftLogger => _logger ??= ScopedServices.GetRequiredService<ILoggerFactory>().CreateLogger(GetType());
        /// <summary>
        /// 显示操作成功逻辑，具体项目使用自己的UI框架提示
        /// </summary>
        protected virtual ValueTask ShowSuccessMessage(string title = "操作提示", string msg = "操作成功！") => ValueTask.CompletedTask;
        /// <summary>
        /// 显示操作失败逻辑，具体项目使用自己的UI框架提示
        /// </summary>
        protected virtual ValueTask ShowFailMessage(string title = "操作提示", string msg = "操作失败！") => ValueTask.CompletedTask;
    }
}
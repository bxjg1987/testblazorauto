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
    public class CommonBaseComponent : OwningComponentBase
    {
        private Zhongjie zhongjieGlobal;
        /// <summary>
        /// 获取变形精怪中介，整个应用全局的
        /// </summary>
        protected virtual Zhongjie GlobalZhongjie => zhongjieGlobal ??= ScopedServices.GetRequiredService<Zhongjie>();

        /// <summary>
        /// 级联的事件总线，外层组件所处的层次决定此事件总线的范围
        /// </summary>
        [CascadingParameter]
        protected virtual Zhongjie? OwnZhongjie { get; set; }

        private ILogger _logger;
        /// <summary>
        /// 日志
        /// </summary>
        protected virtual ILogger MicrosoftLogger
        {
            get
            {
                if (_logger == default)
                    _logger = ScopedServices.GetRequiredService<ILoggerFactory>().CreateLogger(GetType());
                return _logger;
            }
        }
    }
}
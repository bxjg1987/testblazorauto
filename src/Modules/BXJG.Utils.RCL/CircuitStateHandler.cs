using Abp.Runtime.Session;
using BXJG.Common;
using BXJG.Common.RCL;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils
{
    /// <summary>
    /// blazor server 线路处理器，它将线路及其关联数据存储到单例的CircuitStateContainer，以此来使用线路中共享数据
    /// 由于它是scope注册的，组件中可以直接注入它
    /// </summary>
    public class CircuitStateHandler : CircuitHandler
    {
        CircuitStateContainer container;
        Microsoft.Extensions.Logging.ILoggerFactory loggerFactory;
        IAbpSession session;
        ILogger logger;

        public Circuit Current { get; private set; }

        public CircuitStateHandler(CircuitStateContainer container, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory, IAbpSession session, ILogger logger)
        {
            this.container = container;
            this.loggerFactory = loggerFactory;
            this.session = session;
            this.logger = logger;
        }

        public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            Current = circuit;
            container.Add(circuit, new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase) {
                { CircuitStateContainer.userId , session.UserId},
                { CircuitStateContainer.zhongjie, new Zhongjie(loggerFactory)},
            });
            logger.Debug($"blazor server上线，用户id：{session.UserId} 此用户的线路数量：{container.GetByUserId(session.UserId).Count()} 总线路容器数量：{container.Count}");

            return base.OnConnectionUpAsync(circuit, cancellationToken); //担心父类此方法会修改，所以调用下更保险

        }
        public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            container.GetZhongjie(circuit)?.Zhuxiao();
            container.Remove(circuit);
            logger.Debug($"blazor server下线，用户id：{session.UserId}  此用户的线路数量：{container.GetByUserId(session.UserId).Count()} 总线路容器数量：{container.Count}");
            return base.OnConnectionDownAsync(circuit, cancellationToken); //担心父类此方法会修改，所以调用下更保险
        }
    }
}
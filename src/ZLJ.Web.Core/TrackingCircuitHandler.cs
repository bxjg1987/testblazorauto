using Abp.Dependency;
using Abp.Events.Bus;
using BXJG.Common;
using Microsoft.AspNetCore.Components.Server.Circuits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZLJ
{
    //public class TrackingCircuitEventData : Abp.Events.Bus.EventData
    //{

    //}

    //https://learn.microsoft.com/zh-cn/aspnet/core/blazor/fundamentals/signalr?view=aspnetcore-7.0
    /// <summary>
    /// 所有app公用的电路处理器
    /// </summary>
    public class TrackingCircuitHandler : CircuitHandler//,ISingletonDependency
    {

        private readonly Zhongjie zhongjie;
        private HashSet<Circuit> circuits = new();

        public TrackingCircuitHandler(Zhongjie zhongjie)
        {
            this.zhongjie = zhongjie;
        }

        public override async Task OnConnectionUpAsync(Circuit circuit,
            CancellationToken cancellationToken)
        {
            circuits.Add(circuit);
           await  zhongjie.Chufa("TrackingCircuitChanged");
           // return Task.CompletedTask;
        }

        public override async Task OnConnectionDownAsync(Circuit circuit,
            CancellationToken cancellationToken)
        {
            circuits.Remove(circuit);
            await zhongjie.Chufa("TrackingCircuitChanged");
        }

        public int ConnectedCircuits => circuits.Count;
    }
}

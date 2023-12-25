using BXJG.Common;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.Logging;

namespace BXJG.Utils.RCL
{
    public class TrackingCircuitHandler : CircuitHandler, IZhongjieProvider
    {
        static readonly Dictionary<Circuit, Zhongjie> zhongjies = new();
        ILoggerFactory loggerFactory;
        Circuit curr;
        public TrackingCircuitHandler(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }

        //private HashSet<Circuit> circuits = new();

        public override Task OnConnectionUpAsync(Circuit circuit,
            CancellationToken cancellationToken)
        {
            curr = circuit;
            zhongjies.Add(circuit, new Zhongjie(loggerFactory));
            return Task.CompletedTask;
        }

        public override Task OnConnectionDownAsync(Circuit circuit,
            CancellationToken cancellationToken)
        {
            //  circuits.Remove(circuit);
     
            if(zhongjies.TryGetValue(circuit,out var zj))
            { 
            zhongjies.Remove(circuit);
            zj?.Zhuxiao();
            }

            return Task.CompletedTask;
        }

        public Zhongjie GetCurrent()
        {
            if ( curr!=default&& zhongjies.TryGetValue(curr, out var zj))
            {
                return zj;
            }
            return default;
        }

        // public int ConnectedCircuits => circuits.Count;
    }
}

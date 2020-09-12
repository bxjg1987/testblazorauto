using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SuperSocket;
using SuperSocket.Command;
using System.Linq;
using OxygenChamber.Server.Protocol;
using Microsoft.Extensions.Logging;
using SuperSocket.ProtoBase;

namespace OxygenChamber.Server.Command
{
    /// <summary>
    /// 气阀控制
    /// </summary>
    [Command(Key = (byte)103)]
    public class OpenCloseValve : OpenCloseState//, IAsyncCommand<OxygenChamberPackage>
    {
        public OpenCloseValve(ILogger<OpenCloseValve> logger = default)
            : base(3, "气阀", c => c.ElectricState, logger)
        {
        }
    }
}

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
    /// 开关电
    /// </summary>
    [Command(Key = (byte)102)]
    public class OpenCloseElectric : OpenCloseState//, IAsyncCommand<OxygenChamberPackage>
    {
        public OpenCloseElectric(ILogger<OpenCloseElectric> logger)
            : base( 2, "电源", c => c.ElectricState, logger)
        {
        }
    }
}

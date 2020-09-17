using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SuperSocket;
using SuperSocket.Command;
using System.Linq;
using Microsoft.Extensions.Logging;
using SuperSocket.ProtoBase;
using BXJG.Equipment.Protocol;

namespace OxygenChamber.Server.Command
{
    /// <summary>
    /// 气阀控制
    /// </summary>
    [Command(Key = (byte)103)]
    public class OpenCloseValve : OpenCloseState//, IAsyncCommand<OxygenChamberPackage>
    {
        public OpenCloseValve(fszt fszt, ILogger<OpenCloseValve> logger = default)
            : base(3, "气阀", fszt, logger)
        {
        }
    }
}

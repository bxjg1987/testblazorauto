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
    /// 服务器发送开关电的指令时，将由此命令处理
    /// </summary>
    [Command(Key = (byte)103)]
    public class OpenCloseValve : OpenCloseState<OpenCloseValve>//, IAsyncCommand<OxygenChamberPackage>
    {
        public OpenCloseValve(ILogger<OpenCloseValve> logger)
            : base(logger, 3, "气阀", c => c.ElectricState)
        {
        }
    }
}

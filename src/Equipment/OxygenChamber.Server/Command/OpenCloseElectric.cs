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
    /// 开关电
    /// </summary>
    [Command(Key = (byte)102)]
    public class OpenCloseElectric : OpenCloseState//, IAsyncCommand<OxygenChamberPackage>
    {
        public OpenCloseElectric(fszt fszt, ILogger<OpenCloseElectric> logger = default) : base(2, "电源", fszt, logger)
        {
        }
    }
}

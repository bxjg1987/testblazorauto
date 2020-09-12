using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OxygenChamber.Server.Protocol;
using SuperSocket;
using SuperSocket.Command;
using SuperSocket.Server;

namespace OxygenChamber.Server.Command
{
    /// <summary>
    /// 管理端向设备端发送仓压控制的指令的返回值
    /// </summary>
    [Command(Key = (byte)4)]
    public abstract class ChangePressureResult : OpenCloseStateResult
    {
        protected ChangePressureResult(ILogger<ChangePressureResult> logger = null) : base(logger)
        {
        }
    }
}

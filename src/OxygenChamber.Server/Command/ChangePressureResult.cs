using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using OxygenChamber.Server.Protocol;
using SuperSocket;
using SuperSocket.Command;
using SuperSocket.Server;

namespace OxygenChamber.Server.Command
{
    [Command(Key = (byte)4)]
    public abstract class ChangePressureResult : OpenCloseStateResult
    {
    }
}

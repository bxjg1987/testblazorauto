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
    /// 开关电的返回
    /// </summary>
    [Command(Key = (byte)2)]
    public class OpenCloseElectricResult : OpenCloseStateResult
    {
        public OpenCloseElectricResult(ILogger<OpenCloseElectricResult> logger = null) : base(logger)
        {
        }
        //public async ValueTask ExecuteAsync(IAppSession session, OxygenChamberPackage package)
        //{
        //    session["cmdResult" + package.Key] = package;
        //    //前提是在配置SuperSocket时UseInProcSessionContainer()
        //    //var container = (session.Server as IServer).GetAsyncSessionContainer();
        //    //var sessions = await container.GetSessionsAsync();
        //    //var s = session as OxygenChamberSession;

        //    //foreach (var item in sessions)
        //    //{
        //    //   item.SendAsync(Encoding.UTF8.GetBytes("aaa"));
        //    //}

        //}
    }
}

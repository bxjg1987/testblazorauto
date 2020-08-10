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
    /// <summary>
    /// 服务器发送开关门的指令时，将由此命令处理
    /// </summary>
    [Command(Key = (byte)2)]
    public class OpenCloseElectricResult : OpenCloseStateResult
    {
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

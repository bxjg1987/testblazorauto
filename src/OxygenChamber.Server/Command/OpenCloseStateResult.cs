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
    public abstract class OpenCloseStateResult : IAsyncCommand<OxygenChamberPackage>
    {
        public async ValueTask ExecuteAsync(IAppSession session, OxygenChamberPackage package)
        {
            session["cmdResult" + package.Key] = package;
            //前提是在配置SuperSocket时UseInProcSessionContainer()
            //var container = (session.Server as IServer).GetAsyncSessionContainer();
            //var sessions = await container.GetSessionsAsync();
            //var s = session as OxygenChamberSession;
           
            //foreach (var item in sessions)
            //{
            //   item.SendAsync(Encoding.UTF8.GetBytes("aaa"));
            //}
        }
    }
}

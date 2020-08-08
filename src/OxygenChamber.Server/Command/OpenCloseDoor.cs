using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SuperSocket;
using SuperSocket.Command;
using System.Linq;
using OxygenChamber.Server.Protocol;

namespace OxygenChamber.Server.Command
{
    /// <summary>
    /// 服务器发送开关门的指令时，将由此命令处理
    /// </summary>
    [Command(Key = (byte)101)]
    public class OpenCloseDoor : IAsyncCommand<OxygenChamberPackage>
    {
        public async ValueTask ExecuteAsync(IAppSession session, OxygenChamberPackage package)
        {
            //记得在配置SuperSocket时使用UseInProcSessionContainer，否则session容器为空
            var container = (session.Server as IServer).GetAsyncSessionContainer();
            var sessions = await container.GetSessionsAsync();
            //找到目标设备
            var targetSession = sessions.SingleOrDefault(c => c.AsOxygenChamberSession().EquipmentId == package.Id);
            if (targetSession == null)
                throw new Exception($"目标设备未与服务端建立连接，设备Id：{package.Id}");
          // await targetSession.SendAsync();
        }
    }
}

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
            var targetSession = await session.Server.GetSessionByEquipment(package.Id);
            package.Key -= 100;
            await (targetSession as IAppSession).SendAsync(new PackageConverter(), package);
            var dt = DateTimeOffset.Now;
            while ((DateTimeOffset.Now - dt).TotalSeconds < 20)
            {
                await Task.Delay(1);
                var lastInfo = targetSession["cmdResult" + 1] as OxygenChamberPackage;
                if (lastInfo == null || (DateTimeOffset.Now - lastInfo.CreateTime).TotalSeconds > 5)
                    continue;
                await session.SendAsync(new byte[] { 1 });
                await session.Channel.CloseAsync();
                return;
            }
            //设备返回超时
            //throw new Exception("");
            //await session.SendAsync(new byte[] { 0 });
            throw new Exception("超时");
        }
    }
}

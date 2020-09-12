using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using OxygenChamber.Server.Protocol;
using SuperSocket;
using SuperSocket.Command;
using SuperSocket.Server;

namespace OxygenChamber.Server.Command
{
    /// <summary>
    /// 有状态变化时的返回值
    /// </summary>
    public abstract class OpenCloseStateResult : IAsyncCommand<OxygenChamberPackage>
    {
        public ILogger Logger { get; set; } 

        public OpenCloseStateResult(ILogger logger = default)
        {
            Logger = logger ?? NullLogger.Instance;
        }
        public async ValueTask ExecuteAsync(IAppSession session, OxygenChamberPackage package)
        {
            //下发状态查询指令
            var rcmd = new byte[12];
            var j = 0;
            rcmd[j++] = 0xbb;
            rcmd[j++] = 4;
            rcmd[j++] = 0x09;

            var eid = BitConverter.GetBytes(package.EquipmentId);
            rcmd[j++] = eid[3];
            rcmd[j++] = eid[2];
            rcmd[j++] = eid[1];
            rcmd[j++] = eid[0];

            rcmd[j++] = package.PressureControl.ToByte();//状态

            var cy = BitConverter.GetBytes(package.Pressure);
            rcmd[j++] = cy[1];
            rcmd[j++] = cy[0];

            rcmd[j++] = new ReadOnlySpan<byte>(rcmd, 1, 10).CalculateAdd();
            rcmd[j++] = 0xed;

            var targetSession = await session.Server.GetSessionByEquipment(package.EquipmentId);
            await (targetSession as IAppSession).SendAsync(rcmd);

            Logger.LogInformation($"下发指令，通知设备立即上传一次状态");





            //session["cmdResult" + package.Key] = package;
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

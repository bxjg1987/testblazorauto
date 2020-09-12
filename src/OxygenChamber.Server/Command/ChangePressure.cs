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
using Microsoft.Extensions.Logging.Abstractions;

namespace OxygenChamber.Server.Command
{
    /// <summary>
    /// 管理端向设备端发送仓压控制的指令
    /// </summary>
    [Command(Key = (byte)104)]
    public class ChangePressure : IAsyncCommand<OxygenChamberPackage>
    {
        public ILogger Logger { get; set; }

        public ChangePressure(ILogger<ChangePressure> logger = default)
        {
            Logger = logger ?? NullLogger<ChangePressure>.Instance;
        }

        public async ValueTask ExecuteAsync(IAppSession session, OxygenChamberPackage package)
        {
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

            Logger.LogInformation($"下发调整气压指令！设备ID：{package.EquipmentId}，状态：{package.PressureControl},值：{package.Pressure}");

            //var dt = DateTimeOffset.Now;
            //while ((DateTimeOffset.Now - dt).TotalSeconds < 10)
            //{
            //    await Task.Delay(1);
            //    var lastInfo = targetSession["cmdResult" + 4] as OxygenChamberPackage;
            //    if (lastInfo == null || (DateTimeOffset.Now - lastInfo.CreateTime).TotalSeconds > 5)
            //        continue;
            //    await session.SendEquipmentStateAsync(package.EquipmentId,104, lastInfo.PressureState);
            //    await session.Channel.CloseAsync();//为毛session没有关闭方法？
            //    logger.LogInformation($"调整气压成功！设备ID：{package.EquipmentId}，状态：{package.PressureControl},值：{package.Pressure}");
            //    return;
            //}
            ////经过测试，异常时将自动断开连接
            ////await session.SendAsync(new byte[] { 0 });
            //throw new TimeoutException($"调整气压返回结果时超时！设备Id：{package.EquipmentId}，状态：{package.PressureControl},值：{package.Pressure}");
        }
    }
}

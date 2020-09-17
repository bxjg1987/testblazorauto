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
    /// 管理端向设备发送开关门
    /// </summary>
    [Command(Key = (byte)101)]
    public class OpenCloseDoor : OpenCloseState//, IAsyncCommand<OxygenChamberPackage>
    {
        public OpenCloseDoor(fszt fszt, ILogger<OpenCloseDoor> logger = default)
            : base(1, "舱门", fszt, logger)
        {
        }

        //public async ValueTask ExecuteAsync(IAppSession session, OxygenChamberPackage package)
        //{
        //    var targetSession = await session.Server.GetSessionByEquipment(package.EquipmentId);
        //    //package.Key -= 100;
        //    await (targetSession as IAppSession).SendEquipmentStateAsync(package.EquipmentId, 1, package.DoorState);
        //    logger.LogInformation($"下发打开舱门的指令！设备ID：{package.EquipmentId}，状态：{package.DoorState}");
        //    var dt = DateTimeOffset.Now;
        //    while ((DateTimeOffset.Now - dt).TotalSeconds < 10)
        //    {
        //        await Task.Delay(1);
        //        var lastInfo = targetSession["cmdResult" + 1] as OxygenChamberPackage;
        //        if (lastInfo == null || (DateTimeOffset.Now - lastInfo.CreateTime).TotalSeconds > 5)
        //            continue;
        //        //package.Key += 100;
        //        await session.SendEquipmentStateAsync(package.EquipmentId, 101, lastInfo.DoorState);
        //        await session.Channel.CloseAsync();//为毛session没有关闭方法？
        //        logger.LogInformation($"舱门打开成功！设备ID：{package.EquipmentId}，状态：{package.DoorState}");
        //        return;
        //    }
        //    //经过测试，异常时将自动断开连接
        //    //await session.SendAsync(new byte[] { 0 });
        //    throw new TimeoutException($"等待开仓指令返回结果时超时！设备Id：{package.EquipmentId}，状态：{package.DoorState}");
        //}
    }
}

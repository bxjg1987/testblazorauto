using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SuperSocket;
using SuperSocket.Command;
using System.Linq;
using Microsoft.Extensions.Logging;
using SuperSocket.ProtoBase;
using Microsoft.Extensions.Logging.Abstractions;
using BXJG.Equipment;
using BXJG.Equipment.Protocol;

namespace OxygenChamber.Server.Command
{
    /// <summary>
    /// 管理端向设备端发送仓压控制的指令
    /// </summary>
    [Command(Key = (byte)104)]
    public class ChangePressure : IAsyncCommand<OxygenChamberPackage>
    {
        public ILogger Logger { get; set; }
        private readonly fscy fscy;

        public ChangePressure(fscy fscy,ILogger<ChangePressure> logger = default)
        {
            this.fscy = fscy;
            Logger = logger ?? NullLogger<ChangePressure>.Instance;
        }

        public async ValueTask ExecuteAsync(IAppSession session, OxygenChamberPackage package)
        {
            var cykz = package as cykz;
            var targetSession = await session.Server.GetSessionByEquipment(package.EquipmentId);
            await (targetSession as IAppSession).SendAsync(fscy, cykz);

            Logger.LogInformation($"下发调整舱压指令！设备ID：{package.EquipmentId}，状态：{cykz.Add},值：{cykz.Value}");

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

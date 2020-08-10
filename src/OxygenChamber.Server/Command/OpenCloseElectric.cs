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

namespace OxygenChamber.Server.Command
{
    /// <summary>
    /// 服务器发送开关电的指令时，将由此命令处理
    /// </summary>
    [Command(Key = (byte)102)]
    public class OpenCloseElectric : IAsyncCommand<OxygenChamberPackage>
    {
        readonly ILogger<OpenCloseElectric> logger;

        public OpenCloseElectric(ILogger<OpenCloseElectric> logger)
        {
            this.logger = logger;
        }

        public async ValueTask ExecuteAsync(IAppSession session, OxygenChamberPackage package)
        {
            var targetSession = await session.Server.GetSessionByEquipment(package.EquipmentId);
            await (targetSession as IAppSession).SendEquipmentStateAsync(package.EquipmentId, 2, package.ElectricState);
            logger.LogInformation($"下发开关电源的指令！设备ID：{package.EquipmentId}，状态：{package.ElectricState}");
            var dt = DateTimeOffset.Now;
            while ((DateTimeOffset.Now - dt).TotalSeconds < 10)
            {
                await Task.Delay(1);
                var lastInfo = targetSession["cmdResult" + 2] as OxygenChamberPackage;
                if (lastInfo == null || (DateTimeOffset.Now - lastInfo.CreateTime).TotalSeconds > 5)
                    continue;
                await session.SendEquipmentStateAsync(package.EquipmentId, 102, lastInfo.ElectricState);
                await session.Channel.CloseAsync();//为毛session没有关闭方法？
                logger.LogInformation($"电源开关成功！设备ID：{package.EquipmentId}，状态：{package.ElectricState}");
                return;
            }
            //经过测试，异常时将自动断开连接
            //await session.SendAsync(new byte[] { 0 });
            throw new TimeoutException($"等待开电源指令返回结果时超时！设备Id：{package.EquipmentId}，状态：{package.ElectricState}");
        }
    }
}

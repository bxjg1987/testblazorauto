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
    //[Command(Key = (byte)102)]
    public abstract class OpenCloseState<T> : IAsyncCommand<OxygenChamberPackage>
    {
        byte cmdKey;
        string cmdName;
       protected  readonly ILogger<T> logger;
        Func<OxygenChamberPackage, bool> stateAccessor;

        public OpenCloseState(ILogger<T> logger, byte cmdKey, string cmdName, Func<OxygenChamberPackage, bool> stateAccessor)
        {
            this.logger = logger;
            this.cmdKey = cmdKey;
            this.cmdName = cmdName;
            this.stateAccessor = stateAccessor;
        }

        public async ValueTask ExecuteAsync(IAppSession session, OxygenChamberPackage package)
        {
            var targetSession = await session.Server.GetSessionByEquipment(package.EquipmentId);
            await (targetSession as IAppSession).SendEquipmentStateAsync(package.EquipmentId, cmdKey, stateAccessor(package));
            logger.LogInformation($"下发开关{cmdName}指令！设备ID：{package.EquipmentId}，状态：{stateAccessor(package)}");
            var dt = DateTimeOffset.Now;
            while ((DateTimeOffset.Now - dt).TotalSeconds < 10)
            {
                await Task.Delay(1);
                var lastInfo = targetSession["cmdResult" + cmdKey] as OxygenChamberPackage;
                if (lastInfo == null || (DateTimeOffset.Now - lastInfo.CreateTime).TotalSeconds > 5)
                    continue;
                await session.SendEquipmentStateAsync(package.EquipmentId, (byte)(cmdKey + 100), lastInfo.ElectricState);
                await session.Channel.CloseAsync();//为毛session没有关闭方法？
                logger.LogInformation($"{cmdName}开关成功！设备ID：{package.EquipmentId}，状态：{stateAccessor(package)}");
                return;
            }
            //经过测试，异常时将自动断开连接
            //await session.SendAsync(new byte[] { 0 });
            throw new TimeoutException($"等待开电源指令返回结果时超时！设备Id：{package.EquipmentId}，状态：{stateAccessor(package)}");
        }
    }
}

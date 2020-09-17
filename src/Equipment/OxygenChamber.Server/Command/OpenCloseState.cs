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
using BXJG.Equipment.Protocol;

namespace OxygenChamber.Server.Command
{
    /// <summary>
    /// 打开或关闭某个状态的命令抽象类
    /// </summary>
    //[Command(Key = (byte)102)]
    public abstract class OpenCloseState : IAsyncCommand<OxygenChamberPackage>
    {
        byte cmdKey;
        string cmdName;
        public ILogger Logger { get; set; }
     
        readonly fszt fszt;
        public OpenCloseState(byte cmdKey, string cmdName, fszt fszt, ILogger logger = default)
        {
            this.fszt = fszt;
            this.Logger = logger ?? NullLogger.Instance;
            this.cmdKey = cmdKey;
            this.cmdName = cmdName;
        }

        public async ValueTask ExecuteAsync(IAppSession session, OxygenChamberPackage package)
        {
            var zt = package as OxygenChamberStatePackage;
            zt.Key = cmdKey;
            var targetSession = await session.Server.GetSessionByEquipment(package.EquipmentId);
            await (targetSession as IAppSession).SendAsync(  fszt,zt);
            Logger.LogInformation($"下发开关{cmdName}指令！设备ID：{package.EquipmentId}，状态：{zt.State}");
            //var dt = DateTimeOffset.Now;
            //while ((DateTimeOffset.Now - dt).TotalSeconds < 10)
            //{
            //    await Task.Delay(1);
            //    var lastInfo = targetSession["cmdResult" + cmdKey] as OxygenChamberPackage;
            //    if (lastInfo == null || (DateTimeOffset.Now - lastInfo.CreateTime).TotalSeconds > 5)
            //        continue;
            //    await session.SendEquipmentStateAsync(package.EquipmentId, (byte)(cmdKey + 100), lastInfo.ElectricState);
            //    await session.Channel.CloseAsync();//为毛session没有关闭方法？
            //    Logger.LogInformation($"{cmdName}开关成功！设备ID：{package.EquipmentId}，状态：{stateAccessor(package)}");
            //    return;
            //}
            ////经过测试，异常时将自动断开连接
            ////await session.SendAsync(new byte[] { 0 });
            //throw new TimeoutException($"等待开电源指令返回结果时超时！设备Id：{package.EquipmentId}，状态：{stateAccessor(package)}");
        }
    }
}

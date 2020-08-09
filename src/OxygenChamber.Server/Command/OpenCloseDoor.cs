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
    /// 服务器发送开关门的指令时，将由此命令处理
    /// </summary>
    [Command(Key = (byte)101)]
    public class OpenCloseDoor : IAsyncCommand<OxygenChamberPackage>
    {
        readonly ILogger<OpenCloseDoor> logger;
        IPackageEncoder<OxygenChamberPackage> packageDecoder;

        public OpenCloseDoor(ILogger<OpenCloseDoor> logger, IPackageEncoder<OxygenChamberPackage> packageDecoder)
        {
            this.logger = logger;
            this.packageDecoder = packageDecoder;
        }

        public async ValueTask ExecuteAsync(IAppSession session, OxygenChamberPackage package)
        {
            var targetSession = await session.Server.GetSessionByEquipment(package.Id);
            package.Key -= 100;
            await (targetSession as IAppSession).SendAsync(packageDecoder, package);
            logger.LogInformation($"下发打开舱门的指令！设备ID：{package.Id}");
            var dt = DateTimeOffset.Now;
            while ((DateTimeOffset.Now - dt).TotalSeconds < 10)
            {
                await Task.Delay(1);
                var lastInfo = targetSession["cmdResult" + 1] as OxygenChamberPackage;
                if (lastInfo == null || (DateTimeOffset.Now - lastInfo.CreateTime).TotalSeconds > 5)
                    continue;
                package.Key += 100;
                await session.SendAsync(packageDecoder, package);
                await session.Channel.CloseAsync();//为毛session没有关闭方法？
                logger.LogInformation($"舱门打开成功！设备ID：{package.Id}");
                return;
            }
            //经过测试，异常时将自动断开连接
            //await session.SendAsync(new byte[] { 0 });
            throw new TimeoutException($"等待开仓指令返回结果时超时！设备Id：{package.Id}");

        }
    }
}

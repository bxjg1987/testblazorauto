using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BXJG.Equipment.Protocol;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SuperSocket;
using SuperSocket.Command;
namespace OxygenChamber.Server.Command
{
    /// <summary>
    /// 设备发送心跳数据过来时的处理
    /// 心跳包首次抵达时设置session.EquipmentId
    /// </summary>
    [Command(Key = (byte)0)]
    public class HeartbeatResult : IAsyncCommand<OxygenChamberPackage>
    {
        public ILogger Logger { get; set; }

        public HeartbeatResult(ILogger<HeartbeatResult> logger)
        {
            Logger = logger ?? NullLogger<HeartbeatResult>.Instance;
        }

        public async ValueTask ExecuteAsync(IAppSession session, OxygenChamberPackage package)
        {
            Logger.LogInformation($"心跳上报。设备ID：{package.EquipmentId}");
            var mySession = session.AsOxygenChamberSession();
            if (mySession.EquipmentId == default)
                mySession.EquipmentId = package.EquipmentId;
            else if (mySession.EquipmentId != package.EquipmentId)
                throw new Exception($"异常的心跳请求！设备id不匹配，session.EquipmentId：{mySession.EquipmentId}；package.Id：{package.EquipmentId}");
        }
    }
}

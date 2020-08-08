using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OxygenChamber.Server.Protocol;
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
        public async ValueTask ExecuteAsync(IAppSession session, OxygenChamberPackage package)
        {
            var mySession = session.AsOxygenChamberSession();
            if (mySession.EquipmentId == default)
                session.AsOxygenChamberSession().EquipmentId = package.Id;
            else if (mySession.EquipmentId != package.Id)
                throw new Exception($"异常的心跳请求！设备id不匹配，session.EquipmentId：{mySession.EquipmentId}；package.Id：{package.Id}");
        }
    }
}

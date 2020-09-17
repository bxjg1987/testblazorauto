using SuperSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OxygenChamber.Server
{
    public static class Utils
    {
        ///// <summary>
        ///// 转换为16进制的字符串格式
        ///// </summary>
        ///// <param name="bytes"></param>
        ///// <returns></returns>
        //public static string To16String(this ReadOnlySpan<byte> bytes)
        //{
        //    //bytes.ToArray()低效
        //    return BitConverter.ToString(bytes.ToArray(), 0).Replace("-", string.Empty);//.ToLower()
        //}
        /// <summary>
        /// 将supersocket的session转换为我们自定义的session
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static OxygenChamberSession AsOxygenChamberSession(this IAppSession session)
        {
            return session as OxygenChamberSession;
        }
        ///// <summary>
        ///// true 1  false 0
        ///// </summary>
        ///// <param name="b"></param>
        ///// <returns></returns>
        //public static byte ToByte(this bool b)
        //{
        //    return (byte)(b ? 1 : 0);
        //}

        public static async Task<OxygenChamberSession> GetSessionByEquipment(this IServerInfo server, int equipmentId)
        {
            var container = (server as IServer).GetAsyncSessionContainer();
            var sessions = await container.GetSessionsAsync();
            foreach (var item in sessions)
            {
                var p = item as OxygenChamberSession;
                if (p.EquipmentId == equipmentId)
                    return p;
            }
            throw new Exception($"目标设备未与服务端建立连接，设备Id：{equipmentId}");
        }
    }
}

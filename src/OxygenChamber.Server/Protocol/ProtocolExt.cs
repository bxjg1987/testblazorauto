using Microsoft.VisualBasic.CompilerServices;
using SuperSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxygenChamber.Server.Protocol
{
    /// <summary>
    /// 与协议相关的扩展方法
    /// </summary>
    public static class ProtocolExt
    {
        /// <summary>
        /// 根据设备通信协议，检查和校验，若失败则抛异常
        /// </summary>
        /// <param name="buffer">去首尾的字节数组</param>
        public static void CheckCalculateAdd(this ReadOnlySpan<byte> buffer)
        {
            //经过测试用Slice(-1)取尾部会报错 buffer[-1]也不行
            if (!buffer.CalculateAdd().Equals(buffer[buffer.Length - 1]))
                throw new Exception($"校验失败！数据：{buffer.To16String()}");
        }
        /// <summary>
        /// 计算校验值
        /// </summary>
        /// <param name="buffer">不包含头尾的字节数组</param>
        /// <returns></returns>
        public static byte CalculateAdd(this ReadOnlySpan<byte> buffer)
        {
            //和校验位 = （头+命令+长度+协议内容）&  0xff
            int cks = 0xbb;//使用supersocket的头尾协议模板定义的协议会掐头去尾，但是我们的协议要求头部字节参与和校验计算
            for (int i = 0; i < buffer.Length - 1; i++)
            {
                cks += buffer[i];
            }
            var q = cks & 0xff;
            return (byte)q;
        }
        /// <summary>
        /// 向设备下发状态
        /// </summary>
        /// <param name="session"></param>
        /// <param name="equipmentId"></param>
        /// <param name="cmd"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static async ValueTask SendEquipmentStateAsync(this IAppSession session, int equipmentId, byte cmd, bool state)
        {
            var rcmd = new byte[10];
            var j = 0;
            rcmd[j++] = 0xbb;
            rcmd[j++] = cmd;
            rcmd[j++] = 0x07;
            var eid = BitConverter.GetBytes(equipmentId);
            rcmd[j++] = eid[3];
            rcmd[j++] = eid[2];
            rcmd[j++] = eid[1];
            rcmd[j++] = eid[0];
            rcmd[j++] = state.ToByte();//状态
            rcmd[j++] = new ReadOnlySpan<byte>(rcmd, 1, 8).CalculateAdd();
            rcmd[j++] = 0xed;
            await session.SendAsync(rcmd);
        }
    }
}

using Microsoft.VisualBasic.CompilerServices;
using SuperSocket;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BXJG.Equipment;

namespace BXJG.Equipment.Protocol
{
    /// <summary>
    /// 与协议相关的扩展方法
    /// </summary>
    public static class ProtocolExt
    {
        /// <summary>
        /// 和校验，若失败则异常
        /// </summary>
        /// <param name="buffer"></param>
        public static void CheckSum(this ReadOnlySequence<byte> buffer)
        {
            //命令：0xbb;0x01;0x07;0x00;0x01;0x00;0x01;0x00;sum;0xed
            //buffer = 0x01;0x07;0x00;0x01;0x00;0x01;0x00;sum;
            var jyz = buffer.CheckSumValue();
            var jyz2 = buffer.Slice(buffer.Length - 1, 1).FirstSpan[0];

            if (jyz != jyz2)
                throw new Exception($"校验失败！数据：{buffer.ToArray().ToHexStr()}");
        }
        public static byte CheckSumValue(this byte[] buffer)
        {
            return new ReadOnlySequence<byte>(buffer).CheckSumValue();
        }
        public static byte CheckSumValue(this ReadOnlyMemory<byte> buffer)
        {
            return new ReadOnlySequence<byte>(buffer).CheckSumValue();
        }
        public static byte CheckSumValue(this ReadOnlySequence<byte> buffer)
        {
            //命令：0xbb;0x01;0x07;0x00;0x01;0x00;0x01;0x00;sum;0xed
            //buffer = 0x01;0x07;0x00;0x01;0x00;0x01;0x00;sum;
            //和校验位 = （头+命令+长度+协议内容）&  0xff

            var rd = new SequenceReader<byte>(buffer.Slice(0, buffer.Length - 1));
            var cks = 0xbb;//使用supersocket的头尾协议模板定义的协议会掐头去尾，但是我们的协议要求头部字节参与和校验计算
            byte p;

            while (rd.TryRead(out p))
            {
                cks += p;
            }
            var q = cks & 0xFF;
            return (byte)q;
        }

        ///// <summary>
        ///// 向设备下发状态
        ///// </summary>
        ///// <param name="session"></param>
        ///// <param name="equipmentId"></param>
        ///// <param name="cmd"></param>
        ///// <param name="state"></param>
        ///// <returns></returns>
        //public static async ValueTask SendEquipmentStateAsync(this IAppSession session, int equipmentId, byte cmd, bool state)
        //{
        //    var rcmd = new byte[10];
        //    var j = 0;
        //    rcmd[j++] = 0xbb;
        //    rcmd[j++] = cmd;
        //    rcmd[j++] = 0x07;
        //    var eid = BitConverter.GetBytes(equipmentId);
        //    rcmd[j++] = eid[3];
        //    rcmd[j++] = eid[2];
        //    rcmd[j++] = eid[1];
        //    rcmd[j++] = eid[0];
        //    rcmd[j++] = state.ToByte();//状态
        //    rcmd[j++] = new ReadOnlySpan<byte>(rcmd, 1, 8).CalculateAdd();
        //    rcmd[j++] = 0xed;
        //    await session.SendAsync(rcmd);
        //}
    }
}

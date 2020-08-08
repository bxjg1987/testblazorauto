using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public static void CheckCalculateAdd(this ReadOnlySpan< byte> buffer)
        {
            //经过测试用Slice(-1)取尾部会报错 buffer[-1]也不行
            if (buffer.CalculateAdd().Equals(buffer[buffer.Length-1]))
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
            for (int i = 0; i < buffer.Length-1; i++)
            {
                cks += buffer[i];
            }
            var q = cks & 0xff;
            return (byte)q;
        }
    }
}

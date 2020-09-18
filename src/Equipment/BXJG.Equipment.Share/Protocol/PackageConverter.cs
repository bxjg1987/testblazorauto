using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Equipment.Protocol
{
    /// <summary>
    /// 实现二进制与信息包互相转换
    /// </summary>
    public class PackageConverter : IPackageDecoder<OxygenChamberPackage>
    {
        /// <summary>
        /// 将获取到的数据包（不包含头尾字节）的字节数组转换为强类型的数据包
        /// </summary>
        /// <param name="buffer">数据包（不包含头尾字节）的字节数组</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public OxygenChamberPackage Decode(ref ReadOnlySequence<byte> buffer, object context)
        {
            //使用SequenceReader实现高性能的字节处理，因为不涉及到字节缓冲区的创建和拷贝

            buffer.CheckSum();                          //按协议要求校验数据完整性
            var sr = new SequenceReader<byte>(buffer);  //参考https://www.cnblogs.com/jionsoft/archive/2004/01/13/13676277.html
            sr.TryRead(out var cmd);                    //根据协议，第1个byte表示指令类型
            sr.Advance(1);                              //根据协议，第2个字节表示数据包长度，这里忽略它，因为签名已经做了和校验
            sr.TryReadBigEndian(out int equipmentId);   //设备id，如果不对就试试sr.TryReadLittleEndian小端在前
            OxygenChamberPackage r;                     //准备要返回的数据包，但不同指令类型将返回不同子类来表示具体的指令对应的包

            //注意以下对包的解析包含设备和web服务端发送给设备服务端的所有包
            switch (cmd)
            {
                case 0:                                 //心跳包
                    r = new OxygenChamberPackage();
                    break;
                case 104:                               //舱压控制
                    var cykzs = new cykz();
                    r = cykzs;
                    sr.TryRead(out var cykzValue);
                    cykzs.Add = cykzValue == 1;
                    sr.TryReadBigEndian(out short zykzz);
                    cykzs.Value = zykzz;
                    break;
                case 5:                                 //状态上报
                    var ztsbb = new ztsb();
                    r = ztsbb;
                    sr.TryRead(out var zt1);
                    sr.TryRead(out var zt2);
                    sr.TryRead(out var zt3);
                    sr.TryReadBigEndian(out short cy);
                    sr.TryReadBigEndian(out short nd);
                    ztsbb.DoorState = zt1 == 1;
                    ztsbb.ElectricState = zt2 == 1;
                    ztsbb.ValveState = zt3 == 1;
                    ztsbb.Pressure = cy;
                    ztsbb.OxygenConcentration = nd;
                    break;
                default:                                //其余的指令都是状态控制和设备的回复
                    var p = new OxygenChamberStatePackage();
                    r = p;
                    sr.TryRead(out var zt0);
                    p.State = zt0 == 1;
                    break;
            }
            r.Key = cmd;
            r.EquipmentId = equipmentId;

            return r;
        }
    }

    /// <summary>
    /// 将发送的状态变更的包对象序列化为命令的类
    /// </summary>
    public class fszt : IPackageEncoder<OxygenChamberStatePackage>
    {
        const int len = 10;
        public int Encode(IBufferWriter<byte> writer, OxygenChamberStatePackage pack)
        {
            var j = 0;
            byte[] data = new byte[len];
            data[j++] = 0xbb;
            data[j++] = pack.Key;
            data[j++] = len - 3;//按协议 去掉首位和校验
            //System.Buffers.Binary.BinaryPrimitives.big
            var equipmentId = BitConverter.GetBytes(pack.EquipmentId);
            data[j++] = equipmentId[3];
            data[j++] = equipmentId[2];
            data[j++] = equipmentId[1];
            data[j++] = equipmentId[0];
            data[j++] = pack.State.ToByte();
            data[j++] = new ReadOnlyMemory<byte>(data, 1, data.Length - 2).CheckSumValue();
            data[j++] = 0xed;
            writer.Write(data);
            return len;
        }
    }


    /// <summary>
    /// 将舱压包对象序列化为命令的类
    /// </summary>
    public class fscy : IPackageEncoder<cykz>
    {
        const int len = 12;
        public int Encode(IBufferWriter<byte> writer, cykz pack)
        {
            var j = 0;
            byte[] data = new byte[len];
            data[j++] = 0xbb;
            data[j++] = pack.Key;
            data[j++] = len - 3;//按协议 去掉首位和校验
            //System.Buffers.Binary.BinaryPrimitives.big
            var equipmentId = BitConverter.GetBytes(pack.EquipmentId);
            data[j++] = equipmentId[3];
            data[j++] = equipmentId[2];
            data[j++] = equipmentId[1];
            data[j++] = equipmentId[0];
            data[j++] = pack.Add.ToByte();
            var cy = BitConverter.GetBytes(pack.Value);
            data[j++] = cy[1];
            data[j++] = cy[0];
            data[j++] = new ReadOnlyMemory<byte>(data, 1, data.Length - 2).CheckSumValue();
            data[j++] = 0xed;
            writer.Write(data);
            return len;
        }
    }
}

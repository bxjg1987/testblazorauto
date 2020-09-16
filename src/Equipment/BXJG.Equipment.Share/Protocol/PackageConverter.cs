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
    /// 根据协议，将强类型的数据包序列号为byte[]的指令
    /// </summary>
    public class sf : IPackageEncoder<OxygenChamberPackage>
    {
        public int Encode(IBufferWriter<byte> writer, OxygenChamberPackage pack)
        {
            throw new NotImplementedException();
            //byte[] data = null;
            //switch (pack.Key)
            //{
            //    case 101:
            //    case 1:
            //    case 102:
            //    case 2:
            //    case 103:
            //    case 3:
            //        data = new byte[10];
            //        data[2] = 0x07;
            //        if (pack.Key == 1 || pack.Key == 101)
            //            data[7] = pack.DoorState.ToByte();
            //        else if (pack.Key == 2 || pack.Key == 102)
            //            data[7] = pack.ElectricState.ToByte();
            //        else if (pack.Key == 3 || pack.Key == 103)
            //            data[7] = pack.ValveState.ToByte();
            //        break;
            //    case 104:
            //    case 4:
            //        data = new byte[12];
            //        data[2] = 0x09;
            //        data[7] = pack.PressureControl.ToByte();
            //        var pressure = BitConverter.GetBytes(pack.Pressure);
            //        data[8] = pressure[1];
            //        data[9] = pressure[0];
            //        break;
            //    default:
            //        throw new Exception($"无法识别要发送的指令！设备：{pack.EquipmentId}，指令：{pack.Key}");
            //}
            //data[0] = 0xbb;
            //data[1] = pack.Key;
            ////data[2] = 0x07;
            //var equipmentId = BitConverter.GetBytes(pack.EquipmentId);
            //data[3] = equipmentId[3];
            //data[4] = equipmentId[2];
            //data[5] = equipmentId[1];
            //data[6] = equipmentId[0];
            ////data[7] =状态
            //data[data.Length - 2] = new ReadOnlySpan<byte>(data, 1, data.Length - 2).CalculateAdd();
            //data[data.Length - 1] = 0xed;

            //writer.Write(data);
            //return data.Length;
        }
    }
}

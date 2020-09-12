using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace OxygenChamber.Server.Protocol
{
    /// <summary>
    /// 实现二进制与信息包互相状态
    /// </summary>
    public class PackageConverter : /*IPackageEncoder<OxygenChamberPackage>,*/ IPackageDecoder<OxygenChamberPackage>
    {
        /// <summary>
        /// 将获取到的数据包（不包含头尾字节）的字节数组转换为强类型的数据包
        /// </summary>
        /// <param name="buffer">数据包（不包含头尾字节）的字节数组</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public OxygenChamberPackage Decode(ref ReadOnlySequence<byte> buffer, object context)
        {
            //因为不熟悉System.IO.Pipelines，所以不晓得怎么把ReadOnlySequence<byte>转换为Span，先就下面的方式处理吧，日后再说

            ReadOnlySpan<byte> data;
            if (buffer.IsSingleSegment)
                data = buffer.FirstSpan;
            else
                data = new ReadOnlySpan<byte>(buffer.ToArray());

            data.CheckCalculateAdd(); //校验
            var c = new OxygenChamberPackage();
            c.Key = data[0];
            c.EquipmentId = BitConverter.ToInt32(new byte[] { data[5], data[4], data[3], data[2] });
            switch (c.Key)
            {
                case 0:
                    break;
                case 101:
                case 1:
                    c.DoorState = data[6] == 1;
                    break;
                case 102:
                case 2:
                    c.ElectricState = data[6] == 1;
                    break;
                case 103:
                case 3:
                    c.ValveState = data[6] == 1;
                    break;
                case 104:
                case 4:
                    c.PressureState = data[6] == 1;
                    break;
                case 5:
                    c.DoorState = data[6] == 1;
                    c.ElectricState = data[7] == 1;
                    c.ValveState = data[8] == 1;
                    c.Pressure = BitConverter.ToInt16(new byte[] { data[10], data[9] });
                    c.OxygenConcentration = BitConverter.ToInt16(new byte[] { data[12], data[11] });
                    break;
                default:
                    throw new Exception($"无法识别命令！数据：{data.To16String()}");
            }
            return c;
        }

        //public int Encode(IBufferWriter<byte> writer, OxygenChamberPackage pack)
        //{
        //    byte[] data = null;
        //    switch (pack.Key)
        //    {
        //        case 101:
        //        case 1:
        //        case 102:
        //        case 2:
        //        case 103:
        //        case 3:
        //            data = new byte[10];
        //            data[2] = 0x07;
        //            if (pack.Key == 1|| pack.Key == 101)
        //                data[7] = pack.DoorState.ToByte();
        //            else if (pack.Key == 2|| pack.Key == 102)
        //                data[7] = pack.ElectricState.ToByte();
        //            else if (pack.Key == 3|| pack.Key == 103)
        //                data[7] = pack.ValveState.ToByte();
        //            break;
        //        case 104:
        //        case 4:
        //            data = new byte[12];
        //            data[2] = 0x09;
        //            data[7] = pack.PressureControl.ToByte();
        //            var pressure = BitConverter.GetBytes(pack.Pressure);
        //            data[8] = pressure[1];
        //            data[9] = pressure[0];
        //            break;
        //        default:
        //            throw new Exception($"无法识别要发送的指令！设备：{pack.EquipmentId}，指令：{pack.Key}");
        //    }
        //    data[0] = 0xbb;
        //    data[1] = pack.Key;
        //    //data[2] = 0x07;
        //    var equipmentId = BitConverter.GetBytes(pack.EquipmentId);
        //    data[3] = equipmentId[3];
        //    data[4] = equipmentId[2];
        //    data[5] = equipmentId[1];
        //    data[6] = equipmentId[0];
        //    //data[7] =状态
        //    data[data.Length - 2] = new ReadOnlySpan<byte>(data,1, data.Length - 2).CalculateAdd();
        //    data[data.Length - 1] = 0xed;

        //    writer.Write(data);
        //    return data.Length;
        //}
    }
}

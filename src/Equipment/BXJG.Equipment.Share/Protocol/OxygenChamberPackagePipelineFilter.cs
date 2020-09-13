using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Equipment.Protocol
{
    public class OxygenChamberPackagePipelineFilter : BeginEndMarkPipelineFilter<OxygenChamberPackage>
    {
        public OxygenChamberPackagePipelineFilter() : base(new byte[] { 0xBB }, new byte[] { 0xED }) { }

        //protected override OxygenChamberPackage DecodePackage(ref ReadOnlySequence<byte> buffer)
        //{
        //    var bytes = buffer.ToArray();
        //    //校验
        //    bytes.CheckCalculateAdd();

        //    var c = new OxygenChamberPackage();
        //    c.Key = bytes[0];
        //    c.Id = bytes[3];
        //    switch (c.Key)
        //    {
        //        case 0:
        //            break;
        //        case 101:
        //        case 1:
        //            c.DoorState = bytes[3] == 1;
        //            break;
        //        case 102:
        //        case 2:
        //            c.ElectricState = bytes[3] == 1;
        //            break;
        //        case 103:
        //        case 3:
        //            c.ValveState = bytes[3] == 1;
        //            break;
        //        case 104:
        //        case 4:
        //            c.PressureState = bytes[3] == 1;
        //            break;
        //        case 5:
        //            c.DoorState = bytes[3] == 1;
        //            c.ElectricState = bytes[4] == 1;
        //            c.ValveState = bytes[5] == 1;
        //            c.Pressure = BitConverter.ToInt16(new byte[] { bytes[7], bytes[6] });
        //            c.OxygenConcentration = BitConverter.ToInt16(new byte[] { bytes[9], bytes[8] });
        //            break;
        //        default:
        //            throw new Exception($"无法识别命令！数据：{bytes.To16String()}");
        //    }
        //    return c;
        //    // return base.DecodePackage(ref buffer);
        //}
    }
}

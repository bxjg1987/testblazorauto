using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace OxygenChamber.Server
{
    /// <summary>
    /// 设备发送过来的消息的解析器
    /// </summary>
    public class OxygenChamberPackagePipelineFilter : BeginEndMarkPipelineFilter<OxygenChamberPackage>
    {
      //  private static ReadOnlyMemory<byte> ks = new byte[] { 0xBB };
        //private static ReadOnlyMemory<byte> js = new byte[] { 0xED };
        public OxygenChamberPackagePipelineFilter() : base(new byte[] { 0xBB }, new byte[] { 0xED })
        {
        }

        protected override OxygenChamberPackage DecodePackage(ref ReadOnlySequence<byte> buffer)
        {
            var bytes = buffer.ToArray();

            var hjy = bytes.CalculateAdd();
            if (hjy != bytes[bytes.Length - 2])
                throw new Exception($"校验失败！数据：{bytes.To16String()}");

            var c = new OxygenChamberPackage();
            c.Key = bytes[0];
            c.Id = bytes[3];
            switch (c.Key)
            {
                case 1:
                    c.DoorState = bytes[3] == 1;
                    break;
                case 2:
                    c.ElectricState = bytes[3] == 1;
                    break;
                case 3:
                    c.ValveState = bytes[3] == 1;
                    break;
                case 4:
                    c.PressureState = bytes[3] == 1;
                    break;
                case 5:
                    c.DoorState = bytes[3] == 1;
                    c.ElectricState = bytes[4] == 1;
                    c.ValveState = bytes[5] == 1;
                    c.Pressure = BitConverter.ToInt16(new byte[] { bytes[7], bytes[6] });
                    c.OxygenConcentration = BitConverter.ToInt16(new byte[] { bytes[9], bytes[8] });
                    break;
                default:
                    c.OriginalCMD = bytes;  //throw new Exception($"无法识别命令！数据：{bytes.To16String()}");
                    break;
            }
            return c;
            // return base.DecodePackage(ref buffer);
        }
    }
}

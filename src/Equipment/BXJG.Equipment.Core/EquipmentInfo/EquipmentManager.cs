using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Equipment.EquipmentInfo
{
    public class EquipmentManager : DomainService
    {
        //public ValueTask ChangeDoorStateAsync(EquipmentInfoEntity entity, bool state)
        //{

        //}

        //private Span<byte> BuildCommand() {
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
        //}
    }
}

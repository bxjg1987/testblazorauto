using System;
using System.Collections.Generic;
using System.Text;
using Abp.Events.Bus;
using Abp.Events.Bus.Entities;

namespace BXJG.Equipment.EquipmentInfo
{
    /// <summary>
    /// 服务端下发改变设备状态的事件
    /// </summary>
    public class ChangeStateEventData : EventData
    {
        public ChangeStateEventData()
        {
        }

        public ChangeStateEventData( string equipmentId, bool state)
        {
            State = state;
            EquipmentId = equipmentId;
        }

        public bool State { get; set; }
        public string EquipmentId { get; set; }

    }
}

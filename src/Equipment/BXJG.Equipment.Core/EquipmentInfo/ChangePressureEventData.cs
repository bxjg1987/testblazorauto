using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Equipment.EquipmentInfo
{
    /// <summary>
    /// 服务端下发调整仓压的事件
    /// </summary>
    public class ChangePressureEventData : EventData
    {
        public ChangePressureEventData()
        {
        }
        /// <summary>
        /// 服务端下发调整仓压的事件
        /// </summary>
        /// <param name="equipmentId">设备id</param>
        /// <param name="state">true增加，false减少</param>
        public ChangePressureEventData(string equipmentId, bool state)
        {
            Add = state;
            EquipmentId = equipmentId;
        }

        public bool Add { get; set; }
        public string EquipmentId { get; set; }
    }
}

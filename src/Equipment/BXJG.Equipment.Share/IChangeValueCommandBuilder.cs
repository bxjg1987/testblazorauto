using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Equipment
{
    /// <summary>
    /// 改变设备指定值的命令生成器
    /// </summary>
    /// <typeparam name="TEquipmentId"></typeparam>
    /// <typeparam name="TIdentityValue"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IChangeValueCommandBuilder<TEquipmentId, TIdentityValue,TValue>
    {
        ValueTask<Memory<byte>> BuildCommandAsync(TEquipmentId equipmentId, TIdentityValue identityState, TValue value, ICommandCalibrator commandCalibrator = default);
    }
}

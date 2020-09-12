using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Equipment
{
    /// <summary>
    /// 阔以用来生产改变设备状态的命令的接口
    /// </summary>
    /// <typeparam name="TEquipmentId">设备唯一id的类型</typeparam>
    /// <typeparam name="TIdentityState">标识哪个状态的类型</typeparam>
    public interface IChangeStateCommandBuilder<TEquipmentId, TIdentityState>
    {
        /// <summary>
        /// 生成改变设备状态的命令
        /// </summary>
        /// <param name="equipmentId">设备id</param>
        /// <param name="identityState">要改变哪个状态</param>
        /// <param name="state">状态</param>
        /// <param name="commandCalibrator">命令校验器，默认不校验</param>
        /// <returns></returns>
        ValueTask<Memory<byte>> BuildCommandAsync(TEquipmentId equipmentId, TIdentityState identityState, bool state, ICommandCalibrator commandCalibrator = default);
    }
}

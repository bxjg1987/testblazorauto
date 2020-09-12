using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Equipment
{
    /// <summary>
    /// 命令校验器
    /// </summary>
    public interface ICommandCalibrator
    {
        /// <summary>
        /// 获取校验值
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        ValueTask<Memory<byte>> GetCheckValueAsync(ReadOnlySpan<byte> cmd);
        /// <summary>
        /// 校验命令，当失败时抛出异常
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns></returns>
        ValueTask CheckAsync(ReadOnlySpan<byte> cmd);
    }
}

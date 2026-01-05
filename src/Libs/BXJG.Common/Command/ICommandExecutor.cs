using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Common.Command
{
    /// <summary>
    /// 命令执行器接口
    /// </summary>
#if !BROWSER
    public interface ICommandExecutor
    {
        /// <summary>
        /// 执行命令并返回完整结果
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="arguments">参数</param>
        /// <param name="workingDirectory">工作目录</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>执行结果</returns>
        Task<CommandExecutionResult> ExecuteAsync(
            string command,
            string arguments = "",
            string? workingDirectory = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 执行命令并流式输出
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="arguments">参数</param>
        /// <param name="workingDirectory">工作目录</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>输出流</returns>
        IAsyncEnumerable<string> ExecuteStreamingAsync(
            string command,
            string arguments = "",
            string? workingDirectory = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 执行命令并通过事件回调输出
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="arguments">参数</param>
        /// <param name="workingDirectory">工作目录</param>
        /// <param name="onOutput">输出回调</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>执行结果</returns>
        Task<CommandExecutionResult> ExecuteWithCallbackAsync(
            string command,
            string arguments = "",
            string? workingDirectory = null,
            Action<CommandOutputEventArgs>? onOutput = null,
            CancellationToken cancellationToken = default);
    }
#endif
}

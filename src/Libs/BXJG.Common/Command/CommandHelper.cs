using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Common.Command
{
    /// <summary>
    /// 命令执行帮助类（静态方法）
    /// </summary>
#if !BROWSER
    public static class CommandHelper
    {
        private static readonly Lazy<ICommandExecutor> _defaultExecutor = new(() => new CommandExecutor());

        /// <summary>
        /// 获取默认命令执行器
        /// </summary>
        public static ICommandExecutor Default => _defaultExecutor.Value;

        /// <summary>
        /// 创建命令执行器
        /// </summary>
        /// <returns>命令执行器</returns>
        public static ICommandExecutor CreateExecutor()
        {
            return new CommandExecutor();
        }

        /// <summary>
        /// 快捷执行命令并返回完整结果
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="arguments">参数</param>
        /// <param name="workingDirectory">工作目录</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>执行结果</returns>
        public static Task<CommandExecutionResult> ExecuteAsync(
            string command,
            string arguments = "",
            string? workingDirectory = null,
            CancellationToken cancellationToken = default)
        {
            return Default.ExecuteAsync(command, arguments, workingDirectory, cancellationToken);
        }

        /// <summary>
        /// 快捷执行命令并流式输出
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="arguments">参数</param>
        /// <param name="workingDirectory">工作目录</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>输出流</returns>
        public static IAsyncEnumerable<string> ExecuteStreamingAsync(
            string command,
            string arguments = "",
            string? workingDirectory = null,
            CancellationToken cancellationToken = default)
        {
            return Default.ExecuteStreamingAsync(command, arguments, workingDirectory, cancellationToken);
        }

        /// <summary>
        /// 快捷执行命令并通过事件回调输出
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="arguments">参数</param>
        /// <param name="workingDirectory">工作目录</param>
        /// <param name="onOutput">输出回调</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>执行结果</returns>
        public static Task<CommandExecutionResult> ExecuteWithCallbackAsync(
            string command,
            string arguments = "",
            string? workingDirectory = null,
            Action<CommandOutputEventArgs>? onOutput = null,
            CancellationToken cancellationToken = default)
        {
            return Default.ExecuteWithCallbackAsync(command, arguments, workingDirectory, onOutput, cancellationToken);
        }

        /// <summary>
        /// 执行 Shell 命令（跨平台）
        /// </summary>
        /// <param name="command">Shell 命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>执行结果</returns>
        public static Task<CommandExecutionResult> ExecuteShellAsync(
            string command,
            CancellationToken cancellationToken = default)
        {
            var (shell, shellArg) = Environment.OSVersion.Platform == PlatformID.Win32NT
                ? ("cmd.exe", $"/c \"{command}\"")
                : ("/bin/sh", $"-c \"{command}\"");

            return Default.ExecuteAsync(shell, shellArg, null, cancellationToken);
        }

        /// <summary>
        /// 执行 Shell 命令并流式输出（跨平台）
        /// </summary>
        /// <param name="command">Shell 命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>输出流</returns>
        public static IAsyncEnumerable<string> ExecuteShellStreamingAsync(
            string command,
            CancellationToken cancellationToken = default)
        {
            var (shell, shellArg) = Environment.OSVersion.Platform == PlatformID.Win32NT
                ? ("cmd.exe", $"/c \"{command}\"")
                : ("/bin/sh", $"-c \"{command}\"");

            return Default.ExecuteStreamingAsync(shell, shellArg, null, cancellationToken);
        }
    }
#endif
}

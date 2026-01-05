using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Common.Command
{
    /// <summary>
    /// 命令执行器实现
    /// </summary>
#if !BROWSER
    public class CommandExecutor : ICommandExecutor
    {
        /// <summary>
        /// 是否将标准错误合并到标准输出
        /// </summary>
        public bool RedirectStandardErrorToOutput { get; set; }

        /// <summary>
        /// 获取命令执行的完整文件名（处理跨平台差异）
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>完整命令路径</returns>
        private string GetFullCommand(string command)
        {
            // 如果命令已经是完整路径，直接返回
            if (Path.IsPathRooted(command) || command.Contains(Path.DirectorySeparatorChar) || command.Contains(Path.AltDirectorySeparatorChar))
            {
                return command;
            }

            // Windows 下添加 .exe 后缀（如果需要）
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                var commandWithExtension = command.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)
                    ? command
                    : $"{command}.exe";
                return commandWithExtension;
            }

            return command;
        }

        /// <summary>
        /// 安全地释放资源
        /// </summary>
        private static void SafeDispose<T>(ref T? disposable) where T : class, IDisposable
        {
            try
            {
                disposable?.Dispose();
            }
            catch
            {
                // 忽略异常
            }
            finally
            {
                disposable = null;
            }
        }

        /// <summary>
        /// 等待进程退出（兼容 netstandard2.1）
        /// </summary>
        private static Task WaitForExitAsync(Process process, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            var tcs = new TaskCompletionSource<bool>();

            void ProcessExited(object? sender, EventArgs e)
            {
                try
                {
                    tcs.TrySetResult(true);
                    process.Exited -= ProcessExited;
                }
                catch
                {
                    // 忽略异常
                }
            }

            try
            {
                process.EnableRaisingEvents = true;
                process.Exited += ProcessExited;

                if (process.HasExited)
                {
                    process.Exited -= ProcessExited;
                    return Task.CompletedTask;
                }

                // 注册取消令牌
                var registration = cancellationToken.Register(() =>
                {
                    try
                    {
                        process.Exited -= ProcessExited;
                        tcs.TrySetCanceled(cancellationToken);
                        if (!process.HasExited)
                        {
                            process.Kill();
                        }
                    }
                    catch
                    {
                        // 忽略异常
                    }
                });

                tcs.Task.ContinueWith(_ =>
                {
                    try
                    {
                        registration.Dispose();
                        process.Exited -= ProcessExited;
                    }
                    catch
                    {
                        // 忽略异常
                    }
                });

                return tcs.Task;
            }
            catch (Exception ex)
            {
                // 确保在异常情况下取消事件订阅
                try
                {
                    process.Exited -= ProcessExited;
                }
                catch
                {
                    // 忽略异常
                }
                
                return Task.FromException(ex);
            }
        }

        /// <inheritdoc/>
        public async Task<CommandExecutionResult> ExecuteAsync(
            string command,
            string arguments = "",
            string? workingDirectory = null,
            CancellationToken cancellationToken = default)
        {
            var result = new CommandExecutionResult();
            var fullCommand = GetFullCommand(command);

            var startInfo = new ProcessStartInfo
            {
                FileName = fullCommand,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            if (!string.IsNullOrEmpty(workingDirectory))
            {
                startInfo.WorkingDirectory = workingDirectory;
            }

            using var process = new Process { StartInfo = startInfo };
            Process? startedProcess = null;

            try
            {
                process.Start();
                startedProcess = process;
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                // 命令不存在或无法启动
                result.ExitCode = -1;
                result.StandardError = $"无法启动命令 '{fullCommand}': {ex.Message}";
                return result;
            }
            catch (Exception ex)
            {
                // 其他异常
                result.ExitCode = -1;
                result.StandardError = $"启动命令 '{fullCommand}' 时发生错误: {ex.Message}";
                return result;
            }

            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    outputBuilder.AppendLine(args.Data);
                }
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    if (RedirectStandardErrorToOutput)
                    {
                        outputBuilder.AppendLine(args.Data);
                    }
                    else
                    {
                        errorBuilder.AppendLine(args.Data);
                    }
                }
            };

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await WaitForExitAsync(process, cancellationToken).ConfigureAwait(false);

            result.ExitCode = process.ExitCode;
            result.StandardOutput = outputBuilder.ToString();
            result.StandardError = errorBuilder.ToString();

            return result;
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<string> ExecuteStreamingAsync(
            string command,
            string arguments = "",
            string? workingDirectory = null,
            [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var fullCommand = GetFullCommand(command);
            var startInfo = new ProcessStartInfo
            {
                FileName = fullCommand,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            if (!string.IsNullOrEmpty(workingDirectory))
            {
                startInfo.WorkingDirectory = workingDirectory;
            }

            // 检查进程启动异常
            Process? process = null;
            Exception? processException = null;
            
            try
            {
                process = new Process { StartInfo = startInfo };
                process.Start();
            }
            catch (Exception ex)
            {
                processException = ex;
            }

            // 如果进程启动失败，返回错误信息
            if (processException != null)
            {
                yield return $"[ERROR] 执行命令时出错: {processException.Message}";
                yield return $"[ERROR] 命令: {fullCommand}";
                yield return $"[ERROR] 参数: {arguments}";
                yield break;
            }

            // 使用 using 确保资源释放
            using (process)
            {
                // 获取流（这里可能抛出异常）
                StreamReader? standardOutput = null;
                StreamReader? standardError = null;
                Exception? streamException = null;

                try
                {
                    standardOutput = process.StandardOutput;
                    standardError = process.StandardError;
                }
                catch (Exception ex)
                {
                    streamException = ex;
                }

                if (streamException != null)
                {
                    yield return $"[ERROR] 获取输出流时出错: {streamException.Message}";
                    yield break;
                }

                // 等待进程完成，同时读取输出
                var processTask = WaitForExitAsync(process, cancellationToken);
                var outputTask = ReadAllLinesAsync(standardOutput, cancellationToken, false);
                var errorTask = RedirectStandardErrorToOutput 
                    ? Task.FromResult<ArraySegment<string>>(ArraySegment<string>.Empty)
                    : ReadAllLinesAsync(standardError, cancellationToken, true);

                // 等待所有任务完成
                await Task.WhenAll(processTask, outputTask, errorTask).ConfigureAwait(false);

                // 输出标准输出
                foreach (var line in outputTask.Result)
                {
                    yield return line;
                }

                // 输出标准错误
                foreach (var line in errorTask.Result)
                {
                    yield return line;
                }
            }
        }

        /// <summary>
        /// 异步读取所有行
        /// </summary>
        private static async Task<ArraySegment<string>> ReadAllLinesAsync(
            StreamReader reader,
            CancellationToken cancellationToken,
            bool isError)
        {
            try
            {
                var lines = new List<string>();
                
                while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
                {
                    var line = await reader.ReadLineAsync().ConfigureAwait(false);
                    if (line != null)
                    {
                        lines.Add(isError ? $"[ERROR] {line}" : line);
                    }
                }

                return new ArraySegment<string>(lines.ToArray());
            }
            catch (Exception ex)
            {
                // 返回错误信息而不是抛出异常
                var errorMessage = isError 
                    ? $"[ERROR] 读取标准错误时出错: {ex.Message}"
                    : $"[ERROR] 读取标准输出时出错: {ex.Message}";
                
                return new ArraySegment<string>(new[] { errorMessage });
            }
        }

        /// <inheritdoc/>
        public async Task<CommandExecutionResult> ExecuteWithCallbackAsync(
            string command,
            string arguments = "",
            string? workingDirectory = null,
            Action<CommandOutputEventArgs>? onOutput = null,
            CancellationToken cancellationToken = default)
        {
            var result = new CommandExecutionResult();
            var fullCommand = GetFullCommand(command);
            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            var startInfo = new ProcessStartInfo
            {
                FileName = fullCommand,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            if (!string.IsNullOrEmpty(workingDirectory))
            {
                startInfo.WorkingDirectory = workingDirectory;
            }

            using var process = new Process { StartInfo = startInfo };

            try
            {
                process.Start();
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                // 命令不存在或无法启动
                result.ExitCode = -1;
                result.StandardError = $"无法启动命令 '{fullCommand}': {ex.Message}";
                return result;
            }
            catch (Exception ex)
            {
                // 其他异常
                result.ExitCode = -1;
                result.StandardError = $"启动命令 '{fullCommand}' 时发生错误: {ex.Message}";
                return result;
            }

            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    outputBuilder.AppendLine(args.Data);
                    onOutput?.Invoke(new CommandOutputEventArgs(args.Data, false));
                }
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    if (RedirectStandardErrorToOutput)
                    {
                        outputBuilder.AppendLine(args.Data);
                        onOutput?.Invoke(new CommandOutputEventArgs(args.Data, false));
                    }
                    else
                    {
                        errorBuilder.AppendLine(args.Data);
                        onOutput?.Invoke(new CommandOutputEventArgs(args.Data, true));
                    }
                }
            };

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await WaitForExitAsync(process, cancellationToken).ConfigureAwait(false);

            result.ExitCode = process.ExitCode;
            result.StandardOutput = outputBuilder.ToString();
            result.StandardError = errorBuilder.ToString();

            return result;
        }
    }
#endif
}

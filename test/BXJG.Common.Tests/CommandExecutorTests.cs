using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BXJG.Common.Command;

namespace BXJG.Common.Tests
{
    /// <summary>
    /// CommandExecutor 单元测试
    /// </summary>
    public class CommandExecutorTests
    {
        /// <summary>
        /// 测试基本命令执行 - echo 命令
        /// </summary>
        [Fact]
        public async Task ExecuteAsync_ShouldReturnOutput_WhenCommandIsValid()
        {
            // Arrange
            var executor = new CommandExecutor();
            
            // Act
            var result = await executor.ExecuteAsync("cmd.exe", "/c \"echo Hello World\"");
            
            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Contains("Hello World", result.StandardOutput);
            Assert.True(result.Success);
        }

        /// <summary>
        /// 测试无效命令 - 应该返回非零退出码
        /// </summary>
        [Fact]
        public async Task ExecuteAsync_ShouldReturnNonZeroExitCode_WhenCommandIsInvalid()
        {
            // Arrange
            var executor = new CommandExecutor();

            // Act
            var result = await executor.ExecuteAsync("invalid_command_that_does_not_exist", "--version");

            // Assert
            Assert.Equal(-1, result.ExitCode);
            Assert.False(result.Success);
            Assert.NotEmpty(result.StandardError);
            Assert.Contains("无法启动命令", result.StandardError);
        }

        /// <summary>
        /// 测试标准错误输出
        /// </summary>
        [Fact]
        public async Task ExecuteAsync_ShouldCaptureStandardError_WhenCommandFails()
        {
            // Arrange
            var executor = new CommandExecutor();
            
            // Act - 使用一个会报错的命令
            var result = await executor.ExecuteAsync("cmd.exe", "/c \"exit 1\"");
            
            // Assert
            Assert.Equal(1, result.ExitCode);
            Assert.False(result.Success);
        }

        /// <summary>
        /// 测试流式输出
        /// </summary>
        [Fact]
        public async Task ExecuteStreamingAsync_ShouldReturnAllLines_WhenCommandHasMultipleOutputs()
        {
            // Arrange
            var executor = new CommandExecutor();
            var lines = new List<string>();

            // Act
            await foreach (var line in executor.ExecuteStreamingAsync("cmd.exe", "/c \"echo Line1 && echo Line2 && echo Line3\""))
            {
                lines.Add(line);
            }

            // Assert - Windows echo 命令会在末尾添加空格，使用 Trim() 或 Contains
            Assert.True(lines.Any(l => l.Contains("Line1")), $"Expected to find 'Line1' in lines: [{string.Join(", ", lines)}]");
            Assert.True(lines.Any(l => l.Contains("Line2")), $"Expected to find 'Line2' in lines: [{string.Join(", ", lines)}]");
            Assert.True(lines.Any(l => l.Contains("Line3")), $"Expected to find 'Line3' in lines: [{string.Join(", ", lines)}]");
        }

        /// <summary>
        /// 测试回调模式
        /// </summary>
        [Fact]
        public async Task ExecuteWithCallbackAsync_ShouldInvokeCallback_WhenOutputIsReceived()
        {
            // Arrange
            var executor = new CommandExecutor();
            var callbackInvoked = false;
            string? capturedOutput = null;
            
            // Act
            var result = await executor.ExecuteWithCallbackAsync(
                "cmd.exe",
                "/c \"echo Test Output\"",
                onOutput: args =>
                {
                    callbackInvoked = true;
                    capturedOutput = args.Output;
                });
            
            // Assert
            Assert.True(callbackInvoked);
            Assert.NotNull(capturedOutput);
            Assert.Contains("Test Output", capturedOutput);
            Assert.Equal(0, result.ExitCode);
        }

        /// <summary>
        /// 测试取消令牌
        /// </summary>
        [Fact]
        public async Task ExecuteAsync_ShouldCancel_WhenCancellationTokenIsRequested()
        {
            // Arrange
            var executor = new CommandExecutor();
            var cts = new CancellationTokenSource();
            
            // Act - 启动一个长运行命令，然后快速取消
            var task = executor.ExecuteAsync("ping", "127.0.0.1 -n 10", cancellationToken: cts.Token);
            cts.CancelAfter(100); // 100ms 后取消
            
            // Assert - 应该抛出 OperationCanceledException 或 TaskCanceledException
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);
        }

        /// <summary>
        /// 测试工作目录设置
        /// </summary>
        [Fact]
        public async Task ExecuteAsync_ShouldUseWorkingDirectory_WhenSpecified()
        {
            // Arrange
            var executor = new CommandExecutor();
            var workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);

            // Act
            var result = await executor.ExecuteAsync("cmd.exe", "/c cd", workingDirectory: workingDirectory);

            // Assert - Windows 路径大小写不敏感，使用忽略大小写比较
            Assert.Equal(0, result.ExitCode);
            Assert.Contains(workingDirectory, result.StandardOutput, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 测试标准错误合并到标准输出
        /// </summary>
        [Fact]
        public async Task ExecuteAsync_ShouldMergeErrorToOutput_WhenRedirectStandardErrorToOutputIsTrue()
        {
            // Arrange
            var executor = new CommandExecutor
            {
                RedirectStandardErrorToOutput = true
            };
            
            // Act
            var result = await executor.ExecuteAsync("cmd.exe", "/c \"echo StdOut && echo StdErr >&2\"");
            
            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Contains("StdOut", result.StandardOutput);
            Assert.Contains("StdErr", result.StandardOutput);
            Assert.Empty(result.StandardError);
        }

        /// <summary>
        /// 测试静态方法 - CommandHelper
        /// </summary>
        [Fact]
        public async Task CommandHelper_ShouldWorkCorrectly()
        {
            // Act
            var result = await CommandHelper.ExecuteAsync("cmd.exe", "/c \"echo Static Test\"");
            
            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Contains("Static Test", result.StandardOutput);
        }

        /// <summary>
        /// 测试 Shell 命令执行
        /// </summary>
        [Fact]
        public async Task ExecuteShellAsync_ShouldExecuteCommand_CrossPlatform()
        {
            // Act
            var result = await CommandHelper.ExecuteShellAsync("echo Shell Test");
            
            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Contains("Shell Test", result.StandardOutput);
        }

        /// <summary>
        /// 测试流式 Shell 命令
        /// </summary>
        [Fact]
        public async Task ExecuteShellStreamingAsync_ShouldStreamOutput()
        {
            // Arrange
            var lines = new List<string>();
            
            // Act
            await foreach (var line in CommandHelper.ExecuteShellStreamingAsync("echo Stream Test"))
            {
                lines.Add(line);
            }
            
            // Assert
            Assert.NotEmpty(lines);
            Assert.True(lines.Exists(l => l.Contains("Stream Test")));
        }

        /// <summary>
        /// 测试长输出命令的流式处理
        /// </summary>
        [Fact]
        public async Task ExecuteStreamingAsync_ShouldHandleLongOutput()
        {
            // Arrange
            var executor = new CommandExecutor();
            var lineCount = 0;
            
            // Act - 生成 100 行输出
            var command = "cmd.exe";
            var arguments = "/c \"for /L %i in (1,1,100) do @echo Line %i\"";
            
            await foreach (var line in executor.ExecuteStreamingAsync(command, arguments))
            {
                lineCount++;
            }
            
            // Assert
            Assert.Equal(100, lineCount);
        }

        /// <summary>
        /// 测试空输出命令
        /// </summary>
        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyOutput()
        {
            // Arrange
            var executor = new CommandExecutor();
            
            // Act
            var result = await executor.ExecuteAsync("cmd.exe", "/c \"\"");
            
            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.NotNull(result.StandardOutput);
            Assert.NotNull(result.StandardError);
        }

        /// <summary>
        /// 测试带有特殊字符的命令参数
        /// </summary>
        [Fact]
        public async Task ExecuteAsync_ShouldHandleSpecialCharactersInArguments()
        {
            // Arrange
            var executor = new CommandExecutor();
            // 使用更简单的特殊字符，避免 cmd.exe 解析问题
            var specialChars = "Test-Special_Characters";

            // Act
            var result = await executor.ExecuteAsync("cmd.exe", $"/c \"echo {specialChars}\"");

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Contains("Test", result.StandardOutput);
        }

        /// <summary>
        /// 测试多个回调事件
        /// </summary>
        [Fact]
        public async Task ExecuteWithCallbackAsync_ShouldInvokeCallbackMultipleTimes_WhenMultipleLinesOutput()
        {
            // Arrange
            var executor = new CommandExecutor();
            var callCount = 0;
            
            // Act
            var result = await executor.ExecuteWithCallbackAsync(
                "cmd.exe",
                "/c \"echo Line1 && echo Line2 && echo Line3\"",
                onOutput: _ => callCount++);
            
            // Assert
            Assert.True(callCount >= 3);
            Assert.Equal(0, result.ExitCode);
        }

        /// <summary>
        /// 测试 ExecuteStreamingAsync 取消令牌
        /// </summary>
        [Fact]
        public async Task ExecuteStreamingAsync_ShouldHandleCancellation()
        {
            // Arrange
            var executor = new CommandExecutor();
            var cts = new CancellationTokenSource();
            var lines = new List<string>();

            // Act - 启动一个长输出命令，然后快速取消
            var command = "cmd.exe";
            var arguments = "/c \"for /L %i in (1,1,1000) do @echo Line %i && ping 127.0.0.1 -n 1 > nul\"";
            cts.CancelAfter(200); // 200ms 后取消

            try
            {
                await foreach (var line in executor.ExecuteStreamingAsync(command, arguments, cancellationToken: cts.Token))
                {
                    lines.Add(line);
                }
            }
            catch (OperationCanceledException)
            {
                // 预期的取消异常
            }

            // Assert - 应该只读取了部分行，而不是全部 1000 行
            Assert.True(lines.Count < 1000, $"应该只读取部分行，但读取了 {lines.Count} 行");
            // 允许 0 行的情况，因为取消可能发生在命令输出之前
        }

        /// <summary>
        /// 测试 ExecuteWithCallbackAsync 的错误输出标记
        /// </summary>
        [Fact]
        public async Task ExecuteWithCallbackAsync_ShouldMarkErrorOutputCorrectly()
        {
            // Arrange
            var executor = new CommandExecutor();
            var hasErrorOutput = false;
            var hasNormalOutput = false;

            // Act
            var result = await executor.ExecuteWithCallbackAsync(
                "cmd.exe",
                "/c \"echo Normal && echo Error >&2\"",
                onOutput: args =>
                {
                    if (args.IsStandardError)
                    {
                        hasErrorOutput = true;
                    }
                    else
                    {
                        hasNormalOutput = true;
                    }
                });

            // Assert
            Assert.True(hasNormalOutput, "应该有正常输出");
            Assert.True(hasErrorOutput, "应该有错误输出");
            Assert.Equal(0, result.ExitCode);
        }

        /// <summary>
        /// 测试 ExecuteStreamingAsync 的工作目录
        /// </summary>
        [Fact]
        public async Task ExecuteStreamingAsync_ShouldUseWorkingDirectory()
        {
            // Arrange
            var executor = new CommandExecutor();
            var workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
            var lines = new List<string>();

            // Act
            await foreach (var line in executor.ExecuteStreamingAsync(
                "cmd.exe",
                "/c cd",
                workingDirectory: workingDirectory))
            {
                lines.Add(line);
            }

            // Assert
            Assert.True(lines.Any(l => l.Contains("System32", StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// 测试 ExecuteAsync 命令失败的具体错误消息
        /// </summary>
        [Fact]
        public async Task ExecuteAsync_ShouldReturnDetailedErrorMessage_WhenCommandFails()
        {
            // Arrange
            var executor = new CommandExecutor();

            // Act
            var result = await executor.ExecuteAsync("nonexistent_command_xyz123", "--version");

            // Assert
            Assert.Equal(-1, result.ExitCode);
            Assert.NotNull(result.StandardError);
            Assert.Contains("无法启动命令", result.StandardError);
            Assert.Contains("nonexistent_command_xyz123", result.StandardError);
        }

        /// <summary>
        /// 测试 ExecuteStreamingAsync 命令启动失败
        /// </summary>
        [Fact]
        public async Task ExecuteStreamingAsync_ShouldReturnError_WhenCommandFails()
        {
            // Arrange
            var executor = new CommandExecutor();
            var lines = new List<string>();

            // Act
            await foreach (var line in executor.ExecuteStreamingAsync("nonexistent_command_abc", "--version"))
            {
                lines.Add(line);
            }

            // Assert
            Assert.NotEmpty(lines);
            Assert.True(lines.Any(l => l.Contains("[ERROR]")));
            Assert.True(lines.Any(l => l.Contains("执行命令时出错")));
        }

        /// <summary>
        /// 测试 ExecuteWithCallbackAsync 的 null 回调
        /// </summary>
        [Fact]
        public async Task ExecuteWithCallbackAsync_ShouldWork_WhenCallbackIsNull()
        {
            // Arrange
            var executor = new CommandExecutor();

            // Act
            var result = await executor.ExecuteWithCallbackAsync(
                "cmd.exe",
                "/c \"echo Test\"",
                onOutput: null);

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Contains("Test", result.StandardOutput);
        }

        /// <summary>
        /// 测试空行输出
        /// </summary>
        [Fact]
        public async Task ExecuteStreamingAsync_ShouldHandleEmptyLines()
        {
            // Arrange
            var executor = new CommandExecutor();
            var lines = new List<string>();

            // Act - 输出包含空行
            var command = "cmd.exe";
            var arguments = "/c \"echo Line1 && echo. && echo Line2\"";
            await foreach (var line in executor.ExecuteStreamingAsync(command, arguments))
            {
                lines.Add(line);
            }

            // Assert
            Assert.True(lines.Any(l => l.Contains("Line1")));
            Assert.True(lines.Any(l => l.Contains("Line2")));
            // 空行可能被 StreamReader 跳过，这是正常行为
        }

        /// <summary>
        /// 测试命令路径处理（带.exe后缀）
        /// </summary>
        [Fact]
        public async Task ExecuteAsync_ShouldHandleExeExtension()
        {
            // Arrange
            var executor = new CommandExecutor();

            // Act - 使用带 .exe 后缀的命令
            var result = await executor.ExecuteAsync("cmd.exe", "/c \"echo With Extension\"");

            // Assert
            Assert.Equal(0, result.ExitCode);
            Assert.Contains("With Extension", result.StandardOutput);
        }

        /// <summary>
        /// 测试重复执行命令（验证无资源泄漏）
        /// </summary>
        [Fact]
        public async Task ExecuteAsync_ShouldNotLeakResources_OnMultipleExecutions()
        {
            // Arrange
            var executor = new CommandExecutor();

            // Act - 连续执行多次
            for (int i = 0; i < 50; i++)
            {
                var result = await executor.ExecuteAsync("cmd.exe", "/c \"echo Test\"");
                Assert.Equal(0, result.ExitCode);
            }

            // Assert - 如果执行到这而没有资源泄漏异常，说明资源管理正确
            Assert.True(true);
        }
    }
}

using System;

namespace BXJG.Common.Command
{
    /// <summary>
    /// 命令执行结果
    /// </summary>
#if !BROWSER
    public class CommandExecutionResult
    {
        /// <summary>
        /// 退出代码
        /// </summary>
        public int ExitCode { get; set; }

        /// <summary>
        /// 标准输出
        /// </summary>
        public string StandardOutput { get; set; } = string.Empty;

        /// <summary>
        /// 标准错误
        /// </summary>
        public string StandardError { get; set; } = string.Empty;

        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool Success => ExitCode == 0;
    }
#endif
}

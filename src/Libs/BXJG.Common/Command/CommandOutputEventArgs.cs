using System;

namespace BXJG.Common.Command
{
    /// <summary>
    /// 命令输出事件参数
    /// </summary>
#if !BROWSER
    public class CommandOutputEventArgs : EventArgs
    {
        /// <summary>
        /// 输出内容
        /// </summary>
        public string Output { get; set; } = string.Empty;

        /// <summary>
        /// 是否为标准错误输出
        /// </summary>
        public bool IsStandardError { get; set; }

        public CommandOutputEventArgs(string output, bool isStandardError = false)
        {
            Output = output;
            IsStandardError = isStandardError;
        }
    }
#endif
}

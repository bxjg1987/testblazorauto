using log4net.Appender;
using log4net.Core;

namespace ZLJ.Web.HostBlazor.Startup
{
    public class BlazorAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            LogLevel logLevel = LogLevel.None;
            if (loggingEvent.Level.Value == 20000)
                logLevel = LogLevel.Trace;
            if (loggingEvent.Level.Value == 30000)
                logLevel = LogLevel.Debug;
            else if (loggingEvent.Level.Value == 40000)
                logLevel = LogLevel.Information;
            else if (loggingEvent.Level.Value == 60000)
                logLevel = LogLevel.Warning;
            else if (loggingEvent.Level.Value == 70000)
                logLevel = LogLevel.Error;
            else if (loggingEvent.Level.Value == 90000)
                logLevel = LogLevel.Critical;

            var msg = loggingEvent.RenderedMessage;

            var msg2 = string.Empty;
            var ex = loggingEvent.ExceptionObject;
            while (ex != default)
            {
                msg2 = ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + msg2;
                ex = ex.InnerException;
            }
            msg += Environment.NewLine + msg2;
            //loggingEvent.MessageObject
            //BlazorServerLoggerExt.Add(logLevel, msg, loggingEvent.LoggerName);
        }
    }

    //public partial class Logger
    //{
    //}
}

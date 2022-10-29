using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Logging
{
    public static class BlazorServerLoggerExt
    {
        public static event Action OnLog;

        public static readonly ConcurrentDictionary<long, string> MsgContainer = new ConcurrentDictionary<long, string>();
        static long msgId = long.MinValue;
        public static void Add(string msg)
        {
            var tempMsgId = Interlocked.Increment(ref msgId);
            MsgContainer.TryAdd(tempMsgId, msg);
            var min = MsgContainer.MinBy(c => c.Key);
            if (tempMsgId - 100000 > min.Key)
                MsgContainer.TryRemove(min.Key, out var _);
            OnLog?.Invoke();
        }

        public static ILoggingBuilder AddBlazorServerLogger( this ILoggingBuilder builder)
        {
           // builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, BlazorServerLoggerProvider>());

            //LoggerProviderOptions.RegisterProviderOptions
            //    <ColorConsoleLoggerConfiguration, ColorConsoleLoggerProvider>(builder.Services);

            return builder;
        }
    }

    public class BlazorServerLogger : ILogger
    {


        // LogMsgContainer logMsgContainer;
        //public BlazorServerLogger()
        //{
        //    this.logMsgContainer = logMsgContainer;
        //}

        public IDisposable BeginScope<TState>(TState state)
        {
            return default;
            //throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
            //throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var msg = formatter(state, exception);

            BlazorServerLoggerExt.Add( msg);
        }
    }
    [ProviderAlias("BlazorServerLogger")]
    public class BlazorServerLoggerProvider : ILoggerProvider
    {
        readonly ConcurrentDictionary<string, BlazorServerLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);
    
        public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName, name => new BlazorServerLogger());

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}

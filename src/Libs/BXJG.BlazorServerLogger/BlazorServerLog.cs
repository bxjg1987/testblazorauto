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
using Microsoft.VisualBasic;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging.Configuration;

namespace Microsoft.Extensions.Logging
{
    public class LogMsg
    {
        public DateTime Time { get; set; }
        public LogLevel Level { get; set; }
        public object ScopeState { get; set; }
        public string Cls { get; set; }
        public string Message { get; set; }

        public object UserState { get; set; }
    }

    public static class BlazorServerLoggerExt
    {
        public static event Action<LogMsg> OnLog;

        public static readonly ConcurrentDictionary<int, LogMsg> MsgContainer = new ConcurrentDictionary<int, LogMsg>();
        // static long msgId = long.MinValue;
        public static void Add(LogLevel level, string msg, string cls = default, object userState = default, object scopeState=default)
        {
            //   var tempMsgId = Interlocked.Increment(ref msgId);
            var msg1 = new LogMsg { Cls = cls, Level = level, Message = msg, Time = DateTime.Now, UserState = userState,ScopeState=scopeState };
            MsgContainer.TryAdd(msg1.GetHashCode(), msg1);
            var min = MsgContainer.MinBy(c => c.Value.Time);
            if (MsgContainer.Count > 100000)
                MsgContainer.TryRemove(min.Key, out var _);
            OnLog?.Invoke(msg1);
        }

        public static ILoggingBuilder AddBlazorServerLogger(this ILoggingBuilder builder)
        {
            // builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, BlazorServerLoggerProvider>());

            LoggerProviderOptions.RegisterProviderOptions<lazorServerLoggerConfiguration, BlazorServerLoggerProvider>(builder.Services);

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

        /*
         * 作用域类似abp中的uow，要做成包洋葱的形式
         * 调用方通常实用using方式
         * state是作用域中的数据，也可以看成作用域标识，简单的话用字符串
         * 一个日志对象通常是tran...的生命周期，可能反复创建作用域
         * 所以要在此对象上存储一个洋葱的数据结构，且要记录当前所处洋葱层次
         * 在下方的log方法中，要取得当前的作用域
         * 看情况是否需要结合AsyncLocal
         * 考虑一颗洋葱在单个线程中有效，问题会更简单
         */

        /// <summary>
        /// 洋葱
        /// </summary>
        class Yangcong : IDisposable, IAsyncDisposable
        {
            /// <summary>
            /// 外层
            /// </summary>
            public Yangcong Outer { get; set; }
            /// <summary>
            /// 子层
            /// 没必要多个子层，前面一个子层用完了，后一个子层才会呗创建
            /// 且唯一有效子层作为当前子层
            /// </summary>
            public Yangcong Inner { get; set; }
            /// <summary>
            /// 当前洋葱片
            /// </summary>
            public static AsyncLocal<Yangcong> Current { get; set; } = new AsyncLocal<Yangcong>();
            /// <summary>
            /// 关联的状态对象
            /// </summary>
            public object State { get; set; }

            public void Dispose()
            {
                Current.Value = Outer;
                Outer.Inner = default;
                if (State is IDisposable m)
                {
                    m?.Dispose();
                }
            }

            public async ValueTask DisposeAsync()
            {
                Current.Value = Outer;
                Outer.Inner = default;
                if (State is IAsyncDisposable m)
                {
                    if (m != default)
                        await m.DisposeAsync();
                }
            }
        }
        string clsName;
        public BlazorServerLogger(string categoryName)
        {
            this.clsName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            var curr = new Yangcong
            {
                Inner = default,
                Outer = Yangcong.Current.Value,
                State = state
            };
            Yangcong.Current.Value = curr;
            return curr;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            //这里应该允许调用方来配置
            return true;
            //throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var msg = formatter(state, exception);

            //这里可以把作用域丢进去 Yangcong.Current 目前没做
            BlazorServerLoggerExt.Add(logLevel, msg, clsName, state, Yangcong.Current.Value?.State);
        }
    }

    //[UnsupportedOSPlatform("browser")] 不咋好控制，这个日志只能用在asp.net core 项目中，且必须启用blazor
    [ProviderAlias("BlazorServerLogger")]
    public class BlazorServerLoggerProvider : ILoggerProvider
    {
        readonly ConcurrentDictionary<string, BlazorServerLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);

        public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName, name => new BlazorServerLogger(name));

        public void Dispose()
        {
            _loggers.Clear();
            //throw new NotImplementedException();
        }
    }
    //关于配置可以去看官方实现的provider的源码 或者asp.net core 6 框架揭秘
    public class lazorServerLoggerConfiguration
    { 
    
    }
}

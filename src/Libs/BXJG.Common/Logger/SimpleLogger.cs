using BXJG.Common.Sundries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Extensions.Logging
{
    /*
     * 有些场景不使用ioc，但需要日志
     * 比如：单例的Zhongjie
     * 
     * 在浏览器中执行时不支持写文件，仅保留console.write
     */

    /// <summary>
    /// 在服务端文本文件里写入日志，在浏览器中控制台写入
    /// 通常在不使用ioc的组件中使用
    /// 为了保持简单，使用条件编译，debug时记录debug及以上日志，在release时，记录warn及以上日志
    /// </summary>
    public class SimpleLogger : ILogger
    {
        public static readonly SimpleLogger Instance = new SimpleLogger();

        static readonly string dir;

        static SimpleLogger()
        {
            // OperatingSystem.IsBrower .net5+用这个
            if (Helpers.IsBrower())
            {
                Console.WriteLine($"SimpleLogger静态构造函数执行，当前环境为：浏览器");
            }
            else
            {
                Console.WriteLine($"SimpleLogger静态构造函数执行，当前环境为：服务器");
                dir = Path.Combine(AppContext.BaseDirectory, "logs");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
        }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            //本就是简单日志记录，仅用于不适应ioc的组件，所以保持简单，不实现scope日志
            return new D();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
#if DEBUG
            return logLevel >= LogLevel.Debug;
#else
            return logLevel >= LogLevel.Warning;
#endif
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            //state为消息内容

            string msg;
            if (formatter != default)
                msg = formatter(state, exception);
            else
            {
                msg = $"{eventId}\t{state}";
                if (exception != default)
                    msg += $"{Environment.NewLine}{exception.Message}{Environment.NewLine}{exception.StackTrace}";
            }

            //浏览器得判断，否则浏览器端不支持的要报错。

            if (!Helpers.IsBrower())
            {
                var f = $"{logLevel}_{DateTime.Now.ToString("yyyyMMdd")}.txt";
                var fp = Path.Combine(dir, f);
                File.AppendAllText(fp, Environment.NewLine + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff") + " " + msg );
            }

            msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff") + " " + logLevel + "\t" + msg;
            if (!Helpers.IsBrower())
            {
                if (logLevel == LogLevel.Warning)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                if (logLevel == LogLevel.Error)
                    Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine(msg);
            if (!Helpers.IsBrower())
            {
                Console.ResetColor();
            }
        }

        class D : IDisposable
        {
            public void Dispose()
            {
                //throw new NotImplementedException();
            }
        }
    }
}

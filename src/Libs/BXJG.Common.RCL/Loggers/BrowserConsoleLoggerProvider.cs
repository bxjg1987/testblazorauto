using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common.RCL.Loggers
{
    public class BrowserConsoleLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new BrowserConsoleLogger(categoryName);
        }

        public void Dispose()
        {
            // 无需释放资源，因为这是 Blazor WebAssembly 客户端应用  
        }
    }

    public class BrowserConsoleLogger : ILogger
    {
        private readonly string _categoryName;
        //IOptionsMonitor<LoggerFilterOptions> optionsMonitor;
        public BrowserConsoleLogger(string categoryName)
        {
            _categoryName = categoryName;
           // this.optionsMonitor = optionsMonitor;
        }
        private class sdf : IDisposable
        {
            public void Dispose()
            {
               // throw new NotImplementedException();
            }
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            // WebAssembly 不支持作用域日志，所以返回空实现  
            //return NullScope.Instance;
            return new sdf();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (_categoryName.StartsWith("Microsoft.AspNetCore"))
                return false;
            // 根据需要调整日志级别  
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var message = formatter(state, exception);
            var finalMessage = $"{_categoryName}: {message}{Environment.NewLine}";

            if (exception != null)
            {
                finalMessage += exception.ToString();
            }

            // 输出到浏览器控制台  
            Console.WriteLine(finalMessage);
        }
    }
}

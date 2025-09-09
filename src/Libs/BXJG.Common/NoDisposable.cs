using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common
{
    public class NoDisposable : IDisposable, IAsyncDisposable
    {
        public static readonly NoDisposable Instance  = new NoDisposable();
        public void Dispose()
        {
            
        }

        public ValueTask DisposeAsync()
        {
#if NET8_0_OR_GREATER
            return ValueTask.CompletedTask;
#endif
            return new ValueTask();
        }
    }
}

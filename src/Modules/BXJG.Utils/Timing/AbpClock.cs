using Abp.Timing;
using BXJG.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Timing
{
    class AbpClock : IClock, Abp.Dependency.ISingletonDependency
    {
        public ValueTask<DateTime> GetNowAsync()
        {
            return new ValueTask<DateTime>(Clock.Now);
        }

        public ValueTask<DateTimeOffset> GetNowOffsetAsync()
        {
            return new ValueTask<DateTimeOffset>(new DateTimeOffset(Clock.Now));
        }
    }
}

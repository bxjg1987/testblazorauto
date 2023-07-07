using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
namespace ZLJ.txdl
{
    public class Class1
    {
        RequestDelegate next;
        IOptionsMonitor<EquipmentControlCenterConfig> opt;
        string[] ary = new string[] { "/api/services/common/TXDLEquipmentInfoProvider", "/api/services/common/TXDLEquipmentEvent" };

        public Class1(RequestDelegate next, IOptionsMonitor<EquipmentControlCenterConfig> opt)
        {
            this.next = next;
            this.opt = opt;
        }

        public Task Invoke(HttpContext context)
        {
            if (!ary.Any(c => context.Request.Path.Value.Contains(c, StringComparison.OrdinalIgnoreCase)))
                return next(context);

            if (context.Request.Host.Host == "127.0.0.1" || context.Request.Host.Host == "localhost")
                return next(context);

            var sf = new Uri(opt.CurrentValue.ApiUrl);
           
            if (sf.Host.Equals(context.Request.Host.Host, StringComparison.OrdinalIgnoreCase))
                return next(context);

            throw new InvalidOperationException("你想干啥？");
        }
    }
}

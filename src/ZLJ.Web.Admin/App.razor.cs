using BXJG.Common;
using Microsoft.Extensions.Logging;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Admin
{
    public partial class App
    {
        Zhongjie zhongjie;
        [Inject]
        public ILoggerFactory loggerFactory { get; set; }
        protected override void OnInitialized()
        {
            zhongjie = new Zhongjie(loggerFactory);
        }

        public void Dispose()
        {
            zhongjie.Zhuxiao();
        }
    }
}

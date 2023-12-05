using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.ClientProxy;

namespace ZLJ.Admin.ClientProxy
{
    public class AdminAppClientProxy : AppCommonClientProxy
    {
        public AdminAppClientProxy(HttpClient client) : base(client)
        {
        }
    }
}

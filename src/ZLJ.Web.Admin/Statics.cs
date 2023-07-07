using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Admin
{
    public static class Statics
    {
        public static IMask ipv4Mask = RegexMask.IPv4();
    }
}

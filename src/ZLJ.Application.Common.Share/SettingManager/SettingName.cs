using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.Share.SettingManager
{
    public class SettingName
    {
        public string Name { get; set; }
    }

    public class SettingName2: SettingName
    {
       public bool FallbackToDefault { get; set; }
    }
}

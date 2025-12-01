using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.AutoMapper
{
    public interface IExtendableDto
    {
        public Dictionary<string,object> ExtensionData { get; set; }
    }
}

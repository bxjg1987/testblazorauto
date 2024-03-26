using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common.Contracts
{

    public interface IExtendableObj
    {
        public dynamic ExtensionData { get; set; }
    }
}

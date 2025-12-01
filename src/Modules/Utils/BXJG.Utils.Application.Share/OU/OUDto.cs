using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.OU
{
    public class OUDto<TChild> : GeneralTreeNodeBaseDto<TChild>
        where TChild: OUDto<TChild>
    {
    }
}

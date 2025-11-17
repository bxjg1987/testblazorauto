using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Share;

namespace ZLJ.Application.Share.OU
{
    public class OUCreateDto: BXJG.Utils.Application.Share.OU.OUCreateDto
    {
      public OUType OUType { get; set; }
        // public OUEditDto Dto { get; set; }
        //public BXJG.Utils.Application.Share.OU.OUCreateDto Dto { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BXJG.Common.Events;

namespace BXJG.Common.RCL.Events
{
    public class ZhongjieProvider : IZhongjieProvider
    {
        Zhongjie zhongjie;

        public ZhongjieProvider(Zhongjie zhongjie)
        {
            this.zhongjie = zhongjie;
        }

        public Zhongjie GetCurrent()
        {
            return zhongjie;
        }
    }
}

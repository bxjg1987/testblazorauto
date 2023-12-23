using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common.RCL.Event
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

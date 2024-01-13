using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Core.ValueObject
{
    public struct PaperCount
    {
        public int A3BlackAndWhite;
        public int A3TrueColor;
        public int A4BlackAndWhite;
        public int A4TrueColor;
        public PaperCount(int a3bh = default, int a3cs = default, int a4bh = default, int a4cs = default)
        {
            A3BlackAndWhite = a3bh;
            A3TrueColor = a3cs;
            A4BlackAndWhite = a4bh;
            A4TrueColor = a4cs;
        }
    }
}

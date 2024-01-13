using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.Post
{
    public class GetListInput
    {
        public long[] OuIds { get; set; }

        public List<string> OuCodes { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Organizations.Dto
{
   public  class MoveInput
    {
        public long? TargetId { get; set; }
        /// <summary>
        /// 源节点id
        /// </summary>
        public long Id { get; set; }
    }
}

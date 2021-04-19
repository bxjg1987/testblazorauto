using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.DynamicAssociateEntity
{
    /// <summary>
    /// 所有需要动态关联到其它实体的类都应该实现此接口
    /// </summary>
    public interface IDynamicAssociateEntity
    {
        public string DynamicAssociateData { get; set; }
    }
}

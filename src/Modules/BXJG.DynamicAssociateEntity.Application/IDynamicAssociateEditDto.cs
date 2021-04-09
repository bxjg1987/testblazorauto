using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.DynamicAssociateEntity
{
    /// <summary>
    /// 所有需要动态关联到其它实体的编辑新增模型都应该实现此接口
    /// </summary>
    public interface IDynamicAssociateEditDto
    {
        public IDictionary<string, object> DynamicAssociateData { get; set; }
    }
}

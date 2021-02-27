using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.BusinessUser
{
    /*
     * 参考：https://gitee.com/bxjg1987/abp/wikis/多种用户类型?sort_id=3639713
     * 不同类型的业务用户实体或聚合根必须实现此接口
     */

    /// <summary>
    /// 所有业务用户都应该实现此接口
    /// </summary>
    public interface IBusinessUserEntity
    {
        long UserId { get; }
    }
}

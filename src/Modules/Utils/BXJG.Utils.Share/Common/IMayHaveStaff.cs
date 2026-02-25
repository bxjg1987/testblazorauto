using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.Share.Common
{
    /*
     * 一个数据通常有创建人，这个abp的接口已经定义了
     * 
     * 但某时候一个数据可能还需要指定一个负责人或叫处理人，
     * 比如工单，创建人是张三，并将此工单分派给李四
     * 在数据权限场景中，一个用户若仅有查看自己数据的权限，那么只有数据是他创建的或他是处理人，这种数据他应该都可以查看
     * 
     * 这种情况，负责人或处理人往往是可空的，比如工单未分配时处理人为空
     */

    public interface IMayHaveStaff
    {
        public long? StaffInfoId { get; }

        public string? StaffInfoName { get;  }
    }
}

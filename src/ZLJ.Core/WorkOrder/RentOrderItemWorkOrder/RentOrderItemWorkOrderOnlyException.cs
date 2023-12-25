using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.WorkOrder.RentOrderItemWorkOrder
{
    /// <summary>
    /// 同一设备，若已存在未拒绝和未完成的工单，则不允许重复
    /// 此异常对象就表示这种设备维修工单状态重复情况
    /// 应用层可以捕获此异常获取具体的重复工单id，当然是可选的，因为它继承UserFriendlyException
    /// </summary>
    public class RentOrderItemWorkOrderOnlyException: UserFriendlyException
    {
        public RentOrderItemWorkOrderOnlyException()
        {
        }

        public RentOrderItemWorkOrderOnlyException(string message, IEnumerable<long> ids = default) : base(message)
        {
            this.Ids = ids;
        }

        public RentOrderItemWorkOrderOnlyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RentOrderItemWorkOrderOnlyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// 同一设备，已存在的为完结的工单id
        /// </summary>
        public IEnumerable<long> Ids { get;  }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    public static class EnumExt
    {
        public static string GetColor(this Status status)
        {
            switch (status)
            {
                //case Status.ToBeConfirmed:
                //    return "#00";
                case Status.ToBeAllocated:
                    return "#0000ff";
                case Status.ToBeProcessed:
                    return "#00ffff";
                case Status.Processing:
                    return "#ff00ff";
                case Status.Completed:
                    return "#00ff00";
                case Status.Rejected:
                    return "#ffff00";
                default:
                    return "#000000";
            }
        }

        public static string GetColor(this UrgencyDegree urgencyDegree)
        {
            switch (urgencyDegree)
            {
                case UrgencyDegree.Urgent:
                    return "#ff0000";
             
                case UrgencyDegree.Dispensable:
                    return "#777777";
                default:
                    return "#000000";
            }
        }
    }
}

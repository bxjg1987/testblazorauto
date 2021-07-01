using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using BXJG.WorkOrder.WorkOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.EmployeeApplication.WorkOrder
{
    public class GetAllInputBase<TGetTotal> : PagedAndSortedResultRequestDto, IShouldNormalize
    where TGetTotal : GetTotalInputBase
    {
        public TGetTotal GetTotalInput { get; set; }

        public virtual void Normalize()
        {
            if (Sorting.IsNullOrEmpty())
                Sorting = "creationtime desc"; //默认最后更新的用户倒叙，因为它可能发生了业务。或者最后登录用户也行
        }
    }
}

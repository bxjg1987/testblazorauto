using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.App.Admin.WorkOrder.Workload.Dto
{
    public class WorkloadGetAllInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public void Normalize()
        {
            if (Sorting.IsNullOrEmpty())
                Sorting = "CreationTime desc";
        }
    }
}

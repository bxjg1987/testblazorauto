using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Equipment.EquipmentInfo
{
    public class EquipmentInfoGetAllInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public void Normalize()
        {
            if (this.Sorting.IsNullOrEmpty())
                this.Sorting = "creationtime desc";
        }
    }
}

using Abp.Application.Services.Dto;
using Abp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Asset
{
    public class GetEquipmentInfoInput : PagedAndSortedResultRequestDto
    {
        public string AreaCode { get; set; }
        public string Keywords { get; set; }
    }
}

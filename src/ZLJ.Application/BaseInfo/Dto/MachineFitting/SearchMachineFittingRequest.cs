using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;

namespace ZLJ.Application.BaseInfo.Dto.MachineFitting
{
    public class SearchMachineFittingRequest : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public long? MachineId { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrEmpty())
                Sorting = "creationTime desc";
        }
    }
}

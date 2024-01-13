using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;

namespace ZLJ.Application.Admin.BaseInfo.Dto.ConsumeMachine
{
    public class SearchConsumeMachineRequest : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public long? ConsumeId { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrEmpty())
                Sorting = "creationTime desc";
        }
    }
}

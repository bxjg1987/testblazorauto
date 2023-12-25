using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;

namespace ZLJ.App.Admin.BaseInfo.Dto.Machine
{
    public class SearchMachineRequest : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public string Name { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrEmpty())
                Sorting = "creationTime desc";
        }
    }
}

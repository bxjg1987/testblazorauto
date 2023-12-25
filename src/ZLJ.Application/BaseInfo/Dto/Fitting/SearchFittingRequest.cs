using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;

namespace ZLJ.App.Admin.BaseInfo.Dto.Fitting
{
    public class SearchFittingRequest : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public void Normalize()
        {
            if (Sorting.IsNullOrEmpty())
                Sorting = "creationTime desc";
        }

        public string Name { get; set; }
    }
}

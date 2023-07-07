using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;

namespace ZLJ.App.Admin.BaseInfo.Dto.Consume
{
    public class SearchConsumeRequest : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public string Name { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrEmpty())
                Sorting = "creationTime desc";
        }
    }
}

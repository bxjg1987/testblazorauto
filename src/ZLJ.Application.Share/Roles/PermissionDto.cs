using Abp.Application.Services.Dto;
using Abp.Authorization;

namespace ZLJ.Application.Share.Roles
{
   // [AutoMapFrom(typeof(Permission))]
    public class PermissionDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }
    }
}

using System.Collections.Generic;
using ZLJ.Roles.Dto;

namespace ZLJ.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}

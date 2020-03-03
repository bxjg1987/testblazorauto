using System.Collections.Generic;
using ZLJ.Roles.Dto;

namespace ZLJ.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}

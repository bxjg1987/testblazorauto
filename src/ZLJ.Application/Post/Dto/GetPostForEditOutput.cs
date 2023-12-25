using System.Collections.Generic;
using ZLJ.App.Admin.Roles.Dto;

namespace ZLJ.App.Admin.Post.Dto
{
    public class GetPostForEditOutput
    {
        public PostEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}
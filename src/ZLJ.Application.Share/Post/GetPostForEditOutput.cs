using System.Collections.Generic;
using ZLJ.Application.Share.Roles;

namespace ZLJ.Application.Share.Post
{
    public class GetPostForEditOutput
    {
        public PostEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.Application.Common.Share.OU;

namespace ZLJ.Application.Common.Share.Roles
{
    public class RoleDto:BXJG.Utils.Application.Share.Roles.RoleDto
    {
        public List<OUSelectDto> Ous { get; set; } = new List<OUSelectDto>();
    }
}

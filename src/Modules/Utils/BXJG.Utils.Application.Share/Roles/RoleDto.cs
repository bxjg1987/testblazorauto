using BXJG.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.Roles
{
    public class RoleDto : IExtendableObj//:RoleSelectDto
    {
        public dynamic ExtensionData { get; set; }
    }
}

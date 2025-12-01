using BXJG.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.Roles
{
    public class RoleCondition:IHaveKeywords
    {
        public bool? IsStatic { get; set; }
        public string?  Keywords { get; set; }
    }
}

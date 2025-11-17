using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.Share.MultiTenancy
{
    public class RegisterTenantOutput
    {
        public int TenantId { get; set; }

        public string TenancyName { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public bool IsTenantActive { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public bool IsEmailConfirmationRequired { get; set; }
    }
}

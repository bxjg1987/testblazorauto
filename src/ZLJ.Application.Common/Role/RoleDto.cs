using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.App.Common.Role
{
    public class RoleDto:EntityDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}

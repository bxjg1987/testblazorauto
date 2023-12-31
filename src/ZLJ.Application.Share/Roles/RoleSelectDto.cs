using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZLJ.Application.Share.Roles
{
    public class RoleSelectDto : ComboboxItemDto
    {
        public string Name { get; set; }
        public RoleSelectDto()
        {
        }

        public RoleSelectDto(string id, string text, string name = null) : base(id, text)
        {
            this.Name = name;
        }
    }
}

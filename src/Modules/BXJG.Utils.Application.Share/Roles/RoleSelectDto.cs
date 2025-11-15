using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.Application.Share.Roles
{
    public class RoleSelectDto : ComboboxItemDto,IEntityDto<int>
    {
        public int Id { get; set; }
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

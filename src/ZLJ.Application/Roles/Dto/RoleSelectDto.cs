using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZLJ.Roles.Dto
{
    public class RoleSelectDto : GeneralTreeComboboxDto<int?>
    {
        public string Name { get; set; }
        public RoleSelectDto()
        {
        }

        public RoleSelectDto(int? id, string text, string name = null) : base(id, text)
        {
            this.Name = name;
        }
    }
}

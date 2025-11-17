using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZLJ.Application.Common.Share.Roles
{
    public class RoleEditDto:BXJG.Utils.Application.Share.Roles.RoleEditDto
    {
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [StringLength(1000)]
        public string? Description { get; set; }
    }
}

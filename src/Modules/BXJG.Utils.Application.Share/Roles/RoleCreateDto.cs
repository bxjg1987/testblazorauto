using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.Roles
{
    //它是抽象层的，为了不破坏具体实现自己的继承层次，这里的模型只定义比编辑模型多的属性，但不继承编辑模型
    //有实现层通过组合的方式复用抽象的dto

    public class RoleCreateDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "Name")]
        [StringLength(32)]
        public virtual string? Name { get; set; }
    }
}

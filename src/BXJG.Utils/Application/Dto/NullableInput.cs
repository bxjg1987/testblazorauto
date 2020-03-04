using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Utils.Application
{
    public class NullableInput
    {
        public bool Nullable { get; set; }
    }

    public class GetEnumByNameInput : NullableInput
    {
        [Required]
        public string EnumTypeName { get; set; }
    }
}

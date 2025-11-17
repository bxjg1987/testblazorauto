using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.Application.Share.OU;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Share;

namespace ZLJ.Application.Share.OU
{
    public class OUDto<T> : BXJG.Utils.Application.Share.OU.OUDto<T>
        where T : OUDto<T>
    {
        public OUType OUType { get; set; }
        [Display(Name = "单位类型")]
        public string OuTypeText { get; set; }
    }

    public class OUDto : OUDto<OUDto>
    {
    }
}

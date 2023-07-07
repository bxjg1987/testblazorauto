using Abp.AutoMapper;
using BXJG.Utils.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Localization;

namespace ZLJ.App.Customer.OU
{
    [AutoMapFrom(typeof(OUEditDto))]
    public class OUDto: GeneralTreeGetTreeNodeBaseDto<OUDto>
    {
        public bool IsActive { get; set; }
    }
}

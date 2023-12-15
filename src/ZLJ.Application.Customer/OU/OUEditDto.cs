using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.App.Customer.OU
{
    [AutoMapFrom(typeof(OUDto))]
    public class OUEditDto:BXJG.Utils.GeneralTree.GeneralTreeNodeEditBaseDto
    {
        public bool IsActive { get; set; } = true;
    }
}

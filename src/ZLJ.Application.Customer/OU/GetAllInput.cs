using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.App.Customer.OU
{
    public class GetAllInput : BXJG.Utils.GeneralTree.GeneralTreeGetTreeInput
    {
        public bool? IsActive { get; set; }

        public string Keywords { get; set; }
    }
}

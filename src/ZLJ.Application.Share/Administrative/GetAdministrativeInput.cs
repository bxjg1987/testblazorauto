using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Share.Enums;

namespace ZLJ.Application.Share.Administrative
{
    public class GetAdministrativeInput : GeneralTreeGetTreeInput, IHaveKeywords
    {
        public AdministrativeLevel? Level { get; set; }

        public string? Keywords { get; set; }
    }
}

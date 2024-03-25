using Abp.Domain.Services;
using BXJG.Utils.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils
{
    public class BXJGBaseDomainService:DomainService
    {
        public BXJGBaseDomainService() {
            base.LocalizationSourceName = BXJGUtilsConsts.LocalizationSourceName;
        }
    }
}

using Abp.Domain.Services;
using Abp.Linq;
using Abp.Threading;
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
        //本项目中严重依赖ef，以便更方便地操作数据库
        //public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;


        public ICancellationTokenProvider CancellationTokenProvider { get; set; }

        public BXJGBaseDomainService() {
            base.LocalizationSourceName = BXJGUtilsConsts.LocalizationSourceName;
        }
    }
}

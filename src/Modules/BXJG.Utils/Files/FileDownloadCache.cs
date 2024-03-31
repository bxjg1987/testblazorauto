using Abp;
using Abp.Dependency;
using Abp.Domain.Entities.Caching;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using BXJG.Utils.Share.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Files
{

    public class DownloadFileCache : MayHaveTenantEntityCache<FileEntity, DownloadFileResult, Guid>, IDownloadFileCache, ITransientDependency
    {
        public DownloadFileCache(ICacheManager cacheManager, IUnitOfWorkManager unitOfWorkManager, IRepository<FileEntity, Guid> repository, string cacheName = null) : base(cacheManager, unitOfWorkManager, repository, cacheName)
        {
        }

      
    }
}

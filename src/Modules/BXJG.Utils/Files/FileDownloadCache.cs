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

    public class FileDownloadCache : MayHaveTenantEntityCache<FileEntity, DownloadFileResult, Guid>, IDownloadFileCache, ITransientDependency
    {
        public FileDownloadCache(ICacheManager cacheManager, IUnitOfWorkManager unitOfWorkManager, IRepository<FileEntity, Guid> repository)
            : base(cacheManager, unitOfWorkManager, repository)
        {
        }
    }
}

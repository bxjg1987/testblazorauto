using Abp.Authorization;
using Abp.Domain.Entities;
using BXJG.Utils.Application.Share.Dtos;
using BXJG.Utils.Application.Share.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.User
{
    //[AbpAuthorize]

    // 虽然啥都没写，不过将来可扩展

    public class UserProviderAppService<TUser,TCondition,TDto> : ProviderBaseAppService<TUser, PagedAndSortedResultRequest<TCondition>, TDto, long>
        where TUser : class, IEntity<long> 
        where TCondition : class, new()
    {
    }
}

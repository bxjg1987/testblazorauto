using BXJG.Utils.Application.Share.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.Post;
using ZLJ.Application.Common.Share.Roles;
using ZLJ.Core.Authorization.Roles;
using ZLJ.Core.BaseInfo.Post;

namespace ZLJ.Application.Common.Role
{
    public class PostProviderAppService : CommonProviderBaseAppService<PostEntity,PagedAndSortedResultRequest<PostCondition>,PostProviderDto,int>
    {
    }
}

using BXJG.Utils.Application.Share.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.Roles;
using ZLJ.Core.Authorization.Roles;

namespace ZLJ.Application.Common.Role
{
    public class RoleProviderAppService : CommonProviderBaseAppService<ZLJ.Core.Authorization.Roles.Role,PagedAndSortedResultRequest<RoleProviderCondition>,RoleForSelectDto,int>
    {
    }
}

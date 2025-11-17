using BXJG.Utils.Application.Share.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Authorization.Users;

namespace ZLJ.Application.Common.Users
{
    public class StaffProviderAppService : CommonProviderBaseAppService<User,
                                                                        PagedAndSortedResultRequest<ZLJ.Application.Common.Share.User.GetAllCondition>,
                                                                        ZLJ.Application.Common.Share.User.UserSelectDto,
                                                                        long>
    {
    }
}

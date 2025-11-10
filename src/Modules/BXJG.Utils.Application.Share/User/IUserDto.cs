using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.User
{
    public interface IUserDto:IUserForSelectDto,IUserEditDto//,IFullAudited<FullAuditedEntity<long>>
    {
    }
}

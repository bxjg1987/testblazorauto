using BXJG.Utils.Application.Share;
using BXJG.Utils.Application.Share.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ZLJ.Application.Common.Share.Kehu
{
    public interface IKehuProviderAppService : IProviderBaseAppService< PagedAndSortedResultRequest<  Condition>, KehuDto, long>
    {
    }
}

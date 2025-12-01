using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share
{
    [RemoteService(IsEnabled = false, IsMetadataEnabled = false)]
    public interface ICrudBaseCombinatorialAppService<TEntity, TCreateInput, TEntityDto> : IApplicationService
    {
        string CreatePermissionName { get; set; }
        string UpdatePermissionName { get; set; }
        string DeletePermissionName { get; set; }

        Action CheckCreatePermissionAct { get; set; }
        Func<TCreateInput, ValueTask> CheckCreateRepeateFunc { get; set; }
        Func<TCreateInput, ValueTask<TEntity>> MapToEntityCreateFunc { get; set; }
        Func<TEntity, ValueTask> MapToEntityFunc { get; set; }
        Func<TEntity, TCreateInput, Task> CreateSaveFunc { get; set; }
        Func<TEntity, Task<TEntityDto>> CreateAfterFunc { get; set; }

        //Task<TEntityDto> CreateAsync(TCreateInput input);
    }
}

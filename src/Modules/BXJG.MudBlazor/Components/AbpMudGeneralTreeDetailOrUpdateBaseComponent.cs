using BXJG.Common.Dto;
using BXJG.Utils.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.AbpMudBlazor.Components
{
    public abstract class AbpMudGeneralTreeDetailOrUpdateBaseComponent<TAppService,
                                                                       TEntityDto,
                                                                       TPrimaryKey,
                                                                       TUpdateInput,
                                                                       TCreateInput,
                                                                       TGetAllInput> : AbpMudBaseComponent
        where TEntityDto : IGeneralTree<TEntityDto>, IExtendableDto
        where TUpdateInput : IGeneralTree<TEntityDto>
        where TAppService : IGeneralTreeBaseAppService<TEntityDto, TCreateInput, TUpdateInput, TGetAllInput>
    {
    }
}

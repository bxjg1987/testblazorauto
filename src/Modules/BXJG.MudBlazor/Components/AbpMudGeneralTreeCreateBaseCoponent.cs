using BXJG.Common.Dto;
using BXJG.Utils.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.AbpMudBlazor.Components
{
    /*
     * 原本这里不需要定义TEditDto、TGetAllInput泛型参数
     * 但abp原本的crudappservice里面是保护完整的curd的，没有分开定义
     * 我们的通用树服务也是保持这种设计方式的，
     * 这也是合理的，对于前后端分离方式来说很直观。
     * 
     * 所以这里强制引入这俩无用的泛型参数，方便 泛型应用服务注入。
     * 否则若去掉这俩泛型参数，实际的应用服务必须实现这里的约束接口，导致应用服务会出现奇怪的方法
     * 因为泛型类中的参数并不都支持协变
     */

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAppService"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TEditDto"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    public class AbpMudGeneralTreeCreateBaseCoponent<TAppService,
                                                     TEntityDto,
                                                     TCreateInput,
                                                     TEditDto,
                                                     TGetAllInput> : AbpMudBaseComponent
        where TCreateInput : IHaveParentId<long> // GeneralTreeNodeEditBaseDto //注意这里约束为TEditDto，这样强制要求继承编辑模型不合理
        where TEntityDto : IGeneralTree<TEntityDto>// GeneralTreeGetTreeNodeBaseDto<TEntityDto>, IExtendableDto//, new()
        //where TEditDto : GeneralTreeNodeEditBaseDto//父类可以对输入做一定的处理
        //where TGetAllInput : GeneralTreeGetTreeInput, new()
        where TAppService : IGeneralTreeBaseAppService<TEntityDto, TCreateInput, TEditDto, TGetAllInput>
    {
    }
}
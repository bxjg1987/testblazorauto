using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Web.Models;
using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GeneralTree
{
    /// <summary>
    /// 数据字典应用服务接口
    /// </summary>
    public interface IUnAuthGeneralTreeAppService : IUnAuthGeneralTreeAppServiceBase<GeneralTreeGetForSelectInput,
                                                                                     GeneralTreeNodeDto,
                                                                                     GeneralTreeGetForSelectInput,
                                                                                     GeneralTreeComboboxDto>
    { }

    /// <summary>
    /// 数据字典应用服务接口
    /// </summary>
    public interface IGeneralTreeAppService : IGeneralTreeAppServiceBase<GeneralTreeDto,
                                                                         GeneralTreeEditDto,
                                                                         GeneralTreeEditDto,
                                                                         BatchOperationInputLong,
                                                                         GeneralTreeGetTreeInput,
                                                                         EntityDto<long>,
                                                                         GeneralTreeNodeMoveInput>
    { }
}

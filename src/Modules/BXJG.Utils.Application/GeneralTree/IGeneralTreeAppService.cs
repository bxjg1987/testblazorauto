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

namespace BXJG.Utils.GeneralTree
{
    /// <summary>
    /// 数据字典应用服务接口
    /// </summary>
    public interface IDataDictionaryProviderAppService : IGeneralTreeProviderBaseAppService<GeneralTreeGetForSelectInput,
                                                                                     GeneralTreeNodeDto,
                                                                                     GeneralTreeGetForSelectInput,
                                                                                     GeneralTreeComboboxDto>
    { }

    /// <summary>
    /// 数据字典应用服务接口
    /// </summary>
    public interface IDataDictionaryAppService : IGeneralTreeBaseAppService<GeneralTreeDto,
                                                                         GeneralTreeEditDto,
                                                                         GeneralTreeEditDto,
                                                                         GeneralTreeGetTreeInput,
                                                                         BatchOperationInputLong,
                                                                         EntityDto<long>,
                                                                         GeneralTreeNodeMoveInput>
    { }
}

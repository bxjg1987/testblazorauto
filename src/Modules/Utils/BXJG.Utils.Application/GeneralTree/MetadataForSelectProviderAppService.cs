

using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.Auth;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.Application.Share.Metadata;
using BXJG.Utils.GeneralTree;
using BXJG.Utils.Metadata;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.GeneralTree
{
    /// <summary>
    /// 数据字典应用服务类
    /// </summary>
    public class MetadataForSelectProviderAppService : GeneralTreeProviderBaseAppService<MetadataEntity, GeneralTreeGetForSelectInput,
                                                                               MetadataForSelectDto,
                                                                               GeneralTreeGetForSelectInput,
                                                                               GeneralTreeComboboxDto>//, IMetadataProviderAppService
    {
        //public MetadataProviderAppService(IRepository<GeneralTreeEntity, long> repository) : base(repository)
        //{ }
    }

}

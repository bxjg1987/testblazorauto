using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.Application.Share.Metadata
{
    public interface IMetadataProviderAppService : IGeneralTreeProviderBaseAppService<GeneralTreeGetForSelectInput,
                                                                                            DataDictionaryForSelectDto,
                                                                                            GeneralTreeGetForSelectInput,
                                                                                            GeneralTreeComboboxDto>
    { }
}

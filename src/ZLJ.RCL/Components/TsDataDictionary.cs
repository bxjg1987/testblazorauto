using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.RCL.Components
{
    //st是selectTree的意思

    /// <summary>
    /// 数据字典树形下拉框
    /// </summary>
    public class TsDataDictionaryForSearch : TreeSelect<GeneralTreeGetForSelectInput,
                                                                 GeneralTreeNodeDto,
                                                                 GeneralTreeGetForSelectInput,
                                                                 GeneralTreeComboboxDto,
                                                                 IDataDictionaryProviderAppService>
    {
    }
}
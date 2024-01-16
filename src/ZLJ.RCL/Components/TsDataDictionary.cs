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
    public class TsDataDictionaryForSearch : TreeSelectForSearch<GeneralTreeGetForSelectInput,
                                                                 GeneralTreeNodeDto,
                                                                 GeneralTreeGetForSelectInput,
                                                                 GeneralTreeComboboxDto,
                                                                 IDataDictionaryProviderAppService>
    {
        //根据parentId决定的，另外数据字典根settings是否合并还没想好，不过起码数据字典是树形的
        //protected override string sy => "请选择数据字典";
       
    }
    /// <summary>
    /// 数据字典树形下拉框
    /// </summary>
    public class TsDataDictionaryForForm : TreeSelectForSearch<GeneralTreeGetForSelectInput,
                                                               GeneralTreeNodeDto,
                                                               GeneralTreeGetForSelectInput,
                                                               GeneralTreeComboboxDto,
                                                               IDataDictionaryProviderAppService>
    {
        //根据parentId决定的，另外数据字典根settings是否合并还没想好，不过起码数据字典是树形的
        //protected override string sy => "请选择数据字典";
    }
}
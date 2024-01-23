using Abp.Application.Services.Dto;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.GeneralTree
{
    /// <summary>
    /// 通用树形的管理页面使用的显示模型
    /// </summary>
    //[AutoMapFrom(typeof(GeneralTreeEntity))]
    public class DataDictionaryDto : GeneralTreeGetTreeNodeBaseDto<DataDictionaryDto>
    {
        [DisplayName("树形")]
        public bool IsTree { get; set; }
        [DisplayName("树形")]
        public string IsTreeText => IsTree ? "是" : "否";// IsTree.ToString().UtilsL() ;
        [DisplayName("系统预设")]
        public bool IsSysDefine { get; set; }
        [DisplayName("系统预设")]
        public string IsSysDefineText => IsSysDefine ? "是" : "否";//.ToString().UtilsL();
    }

    //[AutoMapFrom(typeof(DataDictionaryEntity))]
    //public class AllergySeleteDto : TreeDataBaseDto<AllergySeleteDto>
    //{
    //    public bool IsTree { get; set; }
    //    public string IsTreeText => IsTree ? "是" : "否";
    //    public bool IsSysDefine { get; set; }
    //    public string IsSysDefineText => IsSysDefine ? "是" : "否";
    //}
}

using Abp.Application.Services.Dto;
using System.ComponentModel;
using BXJG.Common.Contracts;
using System.ComponentModel.DataAnnotations;
using BXJG.Utils.Application.Share.GeneralTree;    

namespace ZLJ.Application.Share.TestTree
{
    //Dto需要基础FullAuditEntity，且某些新增的数据可能不希望显示，因此dto不能继承CreateDto

    /// <summary>
    /// 各应用获取 测试树 的数据模型
    ///</summary>
    public class TestTreeDto : GeneralTreeNodeBaseDto<TestTreeDto>, IExtendableObj
    {

        /// <summary>
        /// 名称
        ///</summary>
        [Display(Name="名称")]
        public string? Name { get; set; }

        /// <summary>
        /// 年龄
        ///</summary>
        [Display(Name="年龄")]
        public int? Age { get; set; }

        /// <summary>
        /// 出生日期
        ///</summary>
        [Display(Name="出生日期")]
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 字符串字段1
        ///</summary>
        [Display(Name="字符串字段1")]
        public string StringField1 { get; set; }

        /// <summary>
        /// 状态
        ///</summary>
        [Display(Name="状态")]
        public int Status { get; set; }

        /// <summary>
        /// 测试3
        ///</summary>
        [Display(Name="测试3")]
        public decimal? F2 { get; set; }

        /// <summary>
        /// 测试4
        ///</summary>
        [Display(Name="测试4")]
        public bool F3 { get; set; }
        /// <summary>
        /// 扩展字段，通常仅用于UI
        ///</summary>
        public dynamic ExtensionData { get; set; }
    }
}
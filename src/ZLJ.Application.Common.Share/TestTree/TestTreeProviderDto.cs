using Abp.Application.Services.Dto;
using System.ComponentModel;
using BXJG.Utils.Application.Share.GeneralTree;    

namespace ZLJ.Application.Common.Share.TestTree
{
    /// <summary>
    /// 获取 测试树 以供选择的数据模型
    ///</summary>
    public class TestTreeProviderDto : GeneralTreeNodeForSelectDto<TestTreeProviderDto>
    {

        /// <summary>
        /// 名称
        ///</summary>
        [DisplayName("名称")]
        public string? Name { get; set; }

        /// <summary>
        /// 年龄
        ///</summary>
        [DisplayName("年龄")]
        public int? Age { get; set; }

        /// <summary>
        /// 出生日期
        ///</summary>
        [DisplayName("出生日期")]
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 字符串字段1
        ///</summary>
        [DisplayName("字符串字段1")]
        public string StringField1 { get; set; }

        /// <summary>
        /// 状态
        ///</summary>
        [DisplayName("状态")]
        public int Status { get; set; }

        /// <summary>
        /// 测试3
        ///</summary>
        [DisplayName("测试3")]
        public decimal? F2 { get; set; }

        /// <summary>
        /// 测试4
        ///</summary>
        [DisplayName("测试4")]
        public bool F3 { get; set; }
    }
}
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.GeneralTree;

namespace ZLJ.Application.Common.Share.TestTree
{
    /// <summary>
    /// 获取 测试树 以供选择的条件
    ///</summary>
    public class TestTreeProviderCondition : GeneralTreeGetForSelectInput, IHaveKeywords
    {

        /// <summary>
        /// 模糊搜索的关键字
        ///</summary>
        public string? Keywords { get; set; }

        /// <summary>
        /// 年龄 最小值
        ///</summary>
        public int? AgeMin { get; set; }
        /// <summary>
        /// 年龄 最大值
        ///</summary>
        public int? AgeMax { get; set; }

        /// <summary>
        /// 出生日期 最小值
        ///</summary>
        public DateTime? BirthdayMin { get; set; }
        /// <summary>
        /// 出生日期 最大值
        ///</summary>
        public DateTime? BirthdayMax { get; set; }

        /// <summary>
        /// 测试3 最小值
        ///</summary>
        public decimal? F2Min { get; set; }
        /// <summary>
        /// 测试3 最大值
        ///</summary>
        public decimal? F2Max { get; set; }

        /// <summary>
        /// 状态
        ///</summary>
        public int? Status { get; set; }

        /// <summary>
        /// 测试4
        ///</summary>
        public bool? F3 { get; set; }
    }
}
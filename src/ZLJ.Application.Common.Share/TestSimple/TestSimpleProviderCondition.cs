using BXJG.Common.Contracts;

namespace ZLJ.Application.Common.Share.TestSimple
{
    /// <summary>
    /// 获取 普通数据测试 以供选择的条件
    ///</summary>
    public class TestSimpleProviderCondition: IHaveKeywords
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
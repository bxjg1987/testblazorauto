using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using BXJG.Utils.GeneralTree;    

namespace ZLJ.Core.TestTree
{
    /// <summary>
    /// 测试树
    ///</summary>
    public class TestTreeEntity : GeneralTreeEntity<TestTreeEntity>
    {
        /// <summary>
        /// 名称
        ///</summary>
        public string? Name { get; set; }
        /// <summary>
        /// 年龄
        ///</summary>
        public int? Age { get; set; }
        /// <summary>
        /// 出生日期
        ///</summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 字符串字段1
        ///</summary>
        public string StringField1 { get; set; }
        /// <summary>
        /// 状态
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        /// 测试3
        ///</summary>
        public decimal? F2 { get; set; }
        /// <summary>
        /// 测试4
        ///</summary>
        public bool F3 { get; set; }
    }
}
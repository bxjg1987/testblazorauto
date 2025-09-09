using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Common.Contracts
{
    /// <summary>
    /// 通用树code编码生成规则
    /// </summary>
    public struct CodeRules
    {
       // public static readonly CodeRules Instance = new CodeRules(".", 5);
       /// <summary>
       /// 两个层级之间的间隔符，如：. - _
       /// </summary>
        public  string Spacer ;
        /// <summary>
        /// 单层编码长度，如：5
        /// </summary>
        public  int Length ;
        public CodeRules()
        {
            Spacer = ".";
            Length = 5;
        }
        public CodeRules(string spacer, int length)
        {
            Spacer = spacer;
            Length = length;
        }
    }
}

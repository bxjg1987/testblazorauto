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

        public  string Spacer ;
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BXJG.Common.Sundries
{
    public class Helpers
    {
        /// <summary>
        /// 获取当前方法的名称
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentMethodName()
        {
            var stackTrace = new StackTrace();
            var stackFrame = stackTrace.GetFrame(1); // 获取调用GetCurrentMethodName的上一层方法  
            var methodName = stackFrame.GetMethod().Name;
            return methodName;
        }
    }
}

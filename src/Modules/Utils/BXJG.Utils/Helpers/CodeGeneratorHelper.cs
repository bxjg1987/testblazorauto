using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Helpers
{
    //这里定义的功能并不常用，所以别定义成扩展方法

    public static class CodeGeneratorHelper
    {
        /// <summary>
        /// 反射执行以CodeGenerator打头的方法
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ps"></param>
        public static void CodeGenerator(this object obj, params object[] ps) {
            var thisType = obj.GetType();
            var ms = thisType.GetMethods().Where(x => x.Name.StartsWith("CodeGenerator"));
            foreach (var item in ms)
            {
                item.Invoke(obj,ps);
            }
        }
    }
}

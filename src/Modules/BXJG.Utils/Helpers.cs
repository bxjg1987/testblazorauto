using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Localization;

namespace ZLJ.Core
{
    public static class Helpers
    {
        /// <summary>
        /// 检查枚举值状态
        /// 不符合要求的报错
        /// </summary>
        /// <param name="val">实体枚举值</param>
        /// <param name="target">希望的枚举值，可以或运算</param>
        /// <param name="identity">实体名称，便于提示用户</param>
        /// <exception cref="UserFriendlyException"></exception>
        public static void CheckState(this Enum val, Enum target, string identity ) {
            if (!target.HasFlag(val))
                throw new UserFriendlyException($"操作失败！{identity}状态为【{val.Enum()}】");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common.Contracts
{
    /// <summary>
    /// 包含dynamic类型的扩展数据
    /// </summary>
    public interface IExtendableObj
    {
        /// <summary>
        /// 扩展数据
        /// </summary>
        public dynamic ExtensionData { get; set; }
    }
}

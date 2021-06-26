using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderType
{
    /// <summary>
    /// 工单类型定义
    /// </summary>
    public class WorkOrderTypeDefine
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 本地化名称
        /// </summary>
        public ILocalizableString DisplayName { get; set; }
        /// <summary>
        /// 是否是默认，目前没啥用
        /// </summary>
        public bool IsDefault { get; set; }
        ///// <summary>
        ///// 关联的管理端应用服务类型，考虑直接用name关联
        ///// </summary>
        //public Type ManagerAppServiceType { get; set; }
    }
}

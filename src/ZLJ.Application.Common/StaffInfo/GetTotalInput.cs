using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BXJG.Common.Contracts;

namespace ZLJ.Application.Common.StaffInfo
{
    /// <summary>
    /// 获取员工信息时的条件模型
    /// </summary>
    public class GetTotalInput : IDynamicCondition
    {
        /// <summary>
        /// x
        /// </summary>
        //public List<ConditionFieldDefine> Conditions1
        //{
        //    get { return Conditions?.ToList(); }
        //    set { Conditions = value; }
        //}
        /// <summary>
        /// 公司和部门code
        /// </summary>
        public string OuCode { get; set; }
        /// <summary>
        /// 区域编码
        /// </summary>
        public string AreaCode { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 岗位id
        /// </summary>
        public int? PostId { get; set; }
        public IEnumerable<ConditionFieldDefine> Conditions { get; set; }
    }
}

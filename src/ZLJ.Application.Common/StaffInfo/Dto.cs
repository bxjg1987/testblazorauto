using Abp.Application.Services.Dto;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.StaffInfo
{
    /// <summary>
    /// 员工信息的显示模型
    /// 这个一般用来提供选择用户的数据，尽力少的字段，只要能识别出是哪个用户就行了
    /// 需要访问用户更多字段时，应提供单独根据id查询的接口，也不用使用后台管理端的GetAsync，因为它可能返回用户敏感信息
    /// </summary>
    public class Dto:EntityDto<long>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string GenderText { get; set; }
        /// <summary>
        /// 年龄
        /// 岁数
        /// </summary>
        public int? AgeYears { get; set; }
        /// <summary>
        /// 所属公司和部门
        /// </summary>
        public string OusText { get; set; }
        /// <summary>
        /// 所属岗位
        /// </summary>
        public string PostsText { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }
        ///// <summary>
        ///// 员工编号
        /////暂时不用，以后用的时候再说
        ///// </summary>
        //public string No { get; set; }
        ///// <summary>
        ///// 关联用户Id
        ///// </summary>
        //public long? UserId { get; set; }
        ///// <summary>
        ///// 所属区域Id
        ///// </summary>
        //public long? AreaId { get; set; }
        ///// <summary>
        ///// 所属区域名
        ///// </summary>
        //public string AreaDisplayName { get; set; }
    }
}

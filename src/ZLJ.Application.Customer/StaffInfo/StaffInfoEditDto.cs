using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZLJ.App.Common.Users;

namespace ZLJ.App.Customer.StaffInfo
{
    public class StaffInfoEditDto : EditUserDto// EntityDto<long>
    {
        /// <summary>
        /// 性别
        /// </summary>
        public BXJG.Common.Gender Gender { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 所属部门id
        /// </summary>
        public long OuId { get; set; }
    }
   
    public class StaffInfoCreateDto : CreateUserDto// EntityDto<long>
    {
        /// <summary>
        /// 性别
        /// </summary>
        public BXJG.Common.Gender Gender { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 所属部门id
        /// </summary>
        [Required(ErrorMessage ="请选择部门")]
        public long? OuId { get; set; }
    }
}
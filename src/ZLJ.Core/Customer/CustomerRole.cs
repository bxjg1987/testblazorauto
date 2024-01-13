using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Core.Customer
{
    /// <summary>
    /// 客户那边用户的角色（也是岗位）
    /// </summary>
   // [Table("bxjg_cust_post")]  使用角色表
    [Comment("客户那边用户的角色（也是岗位）")]
    public class CustomerRole : Role
    {
        public const string CustomerAdminRole = "CustomerAdmin";

        public CustomerRole()
        {
        }

        public CustomerRole(int? tenantId, string displayName) : base(tenantId, displayName)
        {
        }

        public CustomerRole(int? tenantId, string name, string displayName) : base(tenantId, name, displayName)
        {
        }
        //public const string CustomerStaffRole = "CustomerStaff";
    }
}
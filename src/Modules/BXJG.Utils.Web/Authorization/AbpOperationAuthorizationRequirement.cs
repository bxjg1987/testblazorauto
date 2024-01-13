using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Web.Authorization
{
        /// <summary>
        /// 基于abp的授权机制的授权要求
        /// OperationAuthorizationRequirement只适合单个全面名称的场景，而abp提供了多个权限检查的场景
        /// </summary>
        public class AbpOperationAuthorizationRequirement : IAuthorizationRequirement
        {
            ///// <summary>
            ///// 单个权限
            ///// 读写Names的第一个值
            ///// </summary>
            //public string Name
            //{
            //    get { return Names.Length > 0 ? Names[0] : default; }
            //    set
            //    {
            //        if (Names.Length > 0)
            //        {
            //            var temp = Names.ToList();
            //            //temp.Remove(value);
            //            temp.Insert(0, value);
            //            Names = temp.ToArray();
            //        }
            //    }
            //}

        //AuthorizationRequirementProvider

            public string[] PermissionNames { get; set; } //= new string[0];
            public bool RequiredAll { get; set; } = false;
        }
}

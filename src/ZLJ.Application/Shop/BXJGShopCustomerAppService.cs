using Abp.Domain.Repositories;
using BXJG.Shop.Customer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.BaseInfo;
using ZLJ.BaseInfo.Administrative;
using ZLJ.MultiTenancy;

namespace ZLJ.Shop
{
    /// <summary>
    /// 后台管理员对顾客信息进行管理的应用服务
    /// </summary>
    public class BXJGShopCustomerAppService : CustomerAppService<User, Role, RoleManager, UserManager>
    {
        public BXJGShopCustomerAppService(IRepository<CustomerEntity, long> repository, RoleManager roleManager, UserManager userManager, IRepository<User, long> userRepository, IRepository<AdministrativeEntity, long> administrativeRepository) : base(repository, roleManager, userManager, userRepository, administrativeRepository)
        {
        }
    }
}

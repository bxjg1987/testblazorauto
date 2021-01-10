using Abp.Application.Services;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.MultiTenancy;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic;
using Abp.Extensions;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using BXJG.Common;
using BXJG.Common.Dto;
using Microsoft.AspNetCore.Identity;
using Abp.IdentityFramework;
using AutoMapper.QueryableExtensions;
using Abp.AutoMapper;

namespace BXJG.Shop.Customer
{
    /*
     * 设计此功能时还没有引用Microsoft.EntityFrameworkCore，它包含很多好用的扩展方法，使用的AsyncQueryableExecuter + system.linq.dynamic 的方式
     * 而abp zero是直接引用的它
     * 可以考虑直接引用Microsoft.EntityFrameworkCore
     * 
     * 最后决定直接在应用层引入Microsoft.EntityFrameworkCore 因为这能提供及大的便利，且像AsNoChangeTracking无法用AsyncQueryableExecuter实现
     * 
     * await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
     * 源码地址 https://github.com/aspnetboilerplate/aspnetboilerplate/blob/c0604b9b1347a3b9581bf97b4cae22db5b6bab1b/src/Abp.ZeroCore/Authorization/Users/AbpUserManager.cs
     * 默认项目模板中 用户新增和注册 都会调用此方法
     * 很费解，看源码 主要是从 abp Settings中获取值 然后赋值给 IdentityOptions选项对象
     * 但是这个选项对象要么是单例 要么是 一个请求一个实例，每次赋值是几个意思？
     * 暂时不纠结了
     */

    /// <summary>
    /// 后台管理员对商城顾客进行管理的抽象服务类
    /// 模块使用方应提供一个子类并指名各泛型的类型
    /// </summary>
    /// <typeparam name="TTenant">租户类型</typeparam>
    /// <typeparam name="TUser">用户类型</typeparam>
    /// <typeparam name="TRole">角色类型</typeparam>
    /// <typeparam name="TTenantManager">租户管理器类型</typeparam>
    /// <typeparam name="TRoleManager">角色管理器类型</typeparam>
    /// <typeparam name="TUserManager">用户管理器类型</typeparam>
    public class CustomerAppService<TUser,
                                    TRole,
                                    TRoleManager,
                                    TUserManager> : AppServiceBase, ICustomerAppService
        where TUser : AbpUser<TUser>, new()
        where TRole : AbpRole<TUser>, new()
        where TRoleManager : AbpRoleManager<TRole, TUser>
        where TUserManager : AbpUserManager<TRole, TUser>
    {
        private readonly TRoleManager RoleManager;
        private readonly TUserManager UserManager;
        private readonly IRepository<CustomerEntity, long> repository;
        private readonly IRepository<TUser, long> userRepository;

        public CustomerAppService(IRepository<CustomerEntity, long> repository, TRoleManager roleManager, TUserManager userManager, IRepository<TUser, long> userRepository)
        {
            this.repository = repository;
            RoleManager = roleManager;
            UserManager = userManager;
            this.userRepository = userRepository;
        }

        public virtual async Task<CustomerDto> CreateAsync(CustomerUpdateDto input)
        {
            #region 创建主程序的用户
            //这个是从abp默认项目模板的用户管理应用服务中抄过来的
            var user = new TUser();
            user.EmailAddress = input.EmailAddress;
            user.Name = input.Name;
            user.Surname = user.Name;
            user.PhoneNumber = input.PhoneNumber;
            user.IsActive = input.IsActive;
            //user.TenantId = AbpSession.TenantId;//好像UserManager会自己处理
            user.IsEmailConfirmed = true;
            user.IsPhoneNumberConfirmed = true;
            user.UserName = input.UserName;
            await UserManager.InitializeOptionsAsync(AbpSession.TenantId);//看顶部注释
            //user.SetNormalizedNames();//这个貌似没必要调了，UserManager内部会处理
            CheckErrors(await UserManager.CreateAsync(user, input.Password));
            //目前不考虑多角色商城会员
            //if (input.RoleNames != null)
            //{
            //    CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            //}
            CurrentUnitOfWork.SaveChanges();//保存后才能拿到新的UserId

            CheckErrors(await UserManager.AddToRoleAsync(user, CoreConsts.CustomerRoleName));

            //return MapToEntityDto(user);
            #endregion

            //映射不太好处理，手动来吧
            var entity = new CustomerEntity
            {
                //Amount = input.Amount,
                Birthday = input.Birthday,
                Gender = input.Gender,
                //Integral = input.Integral,
                //User = user,//下面设置了userId就行了
                UserId = user.Id
            };
            await repository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();//保存后才能拿到新的会员信息自增Id
            return await GetDtoAsync(entity.Id);
        }

        public virtual async Task<CustomerDto> UpdateAsync(CustomerUpdateDto input)
        {
            var entity = await repository.GetAsync(input.Id);

            #region 更新主程序的用户信息
            var user = await UserManager.GetUserByIdAsync(entity.UserId);
            user.EmailAddress = input.EmailAddress;
            user.Name = input.Name;
            user.Surname = user.Name;
            user.PhoneNumber = input.PhoneNumber;
            //user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;
            user.IsPhoneNumberConfirmed = true;
            user.UserName = input.UserName;
            user.IsActive = input.IsActive;
            CheckErrors(await UserManager.UpdateAsync(user));
            //目前不考虑多角色商城会员
            //if (input.RoleNames != null)
            //{
            //    CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            //}

            // return await GetAsync(input);

            #endregion



            //entity.Amount = input.Amount;
            entity.Birthday = input.Birthday;
            entity.Gender = input.Gender;
            //entity.Integral = input.Integral;
            //entity.TenantId = AbpSession.TenantId.Value;


            await repository.UpdateAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return await GetDtoAsync(input.Id);
        }

        public virtual async Task<PagedResultDto<CustomerDto>> GetAllAsync(GetAllCustomersInput input)
        {
            var query = from c in repository.GetAllIncluding(c => c.Area)
                        join u in userRepository.GetAllIncluding(c => c.Roles) on c.UserId equals u.Id
                        select new { c, u };

            //var qq = repository.GetAllIncluding(c => c.Area).Join<TUser>(userRepository.GetAllIncluding(c => c.Roles), c => c.UserId, d =>d.Id, (c,u)=>  );

            query = query.WhereIf(!input.AreaCode.IsNullOrWhiteSpace(), c => c.c.Area.Code.StartsWith(input.AreaCode))
                   .WhereIf(!input.Keywords.IsNullOrEmpty(), c => c.u.Name.Contains(input.Keywords) || c.u.PhoneNumber.Contains(input.Keywords));

            var totalCount = await query.CountAsync();

            if (!input.Sorting.IsNullOrWhiteSpace())
            {
                if (input.Sorting.Contains("FullName "))
                    input.Sorting = "u." + input.Sorting;
                else
                    input.Sorting = "c." + input.Sorting;
                query = query.OrderBy(input.Sorting);
            }

            query = query.PageBy(input);

            var entities = await query.ToListAsync();
            var dtos = entities.Select(c => MapToDto(c.c, c.u)).ToList();

            return new PagedResultDto<CustomerDto>(
                totalCount,
                dtos
            );
        }

        public virtual async Task<CustomerDto> GetAsync(EntityDto<long> input)
        {
            return await GetDtoAsync(input.Id);
        }

        public virtual async Task<BatchOperationResultLong> DeleteBatchAsync(BatchOperationInputLong input)
        {
            var result = new BatchOperationResultLong();
            foreach (var item in input.Ids)
            {
                try
                {
                    await repository.DeleteAsync(item);
                    result.Ids.Add(item);
                }
                catch (Exception ex)
                {
                    base.Logger.Warn($"删除顾客失败，Id：{item}", ex);
                }
            }
            return result;
        }
        /// <summary>
        /// 辅助方法
        /// </summary>
        /// <param name="identityResult"></param>
        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
        /// <summary>
        /// 根据顾客id获取顾客实体和关联的用户实体
        /// </summary>
        /// <param name="id">顾客id</param>
        /// <returns></returns>
        protected virtual async Task<(CustomerEntity customer, TUser user)> GetOneAsync(long id)
        {
            var query = from c in repository.GetAllIncluding(c => c.Area)
                        join u in userRepository.GetAllIncluding(c => c.Roles) on c.UserId equals u.Id
                        select new { c, u };
            var r = await query.SingleAsync();
            return (r.c, r.u);
        }
        /// <summary>
        /// 将一对一的顾客实体和用户实体转换为顾客的查询模型
        /// </summary>
        /// <param name="c">顾客实体</param>
        /// <param name="u">用户实体</param>
        /// <returns></returns>
        protected virtual CustomerDto MapToDto(CustomerEntity c, TUser u)
        {
            var dto = ObjectMapper.Map<CustomerDto>(c);
            ObjectMapper.Map(u, dto);
            return dto;
        }
        protected virtual async Task<CustomerDto> GetDtoAsync(long id)
        {
            var p = await GetOneAsync(id);
            return MapToDto(p.customer, p.user);
        }
    }
}

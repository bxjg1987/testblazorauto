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
using BXJG.Common;
using BXJG.Common.Dto;
using Microsoft.AspNetCore.Identity;
using Abp.IdentityFramework;
using AutoMapper.QueryableExtensions;
using Abp.AutoMapper;
using Abp.Domain.Uow;
using ZLJ.BaseInfo.Administrative;
using Abp.Timing;

namespace BXJG.Shop.Customer
{
    /*
     * 设计此功能时还没有引用Microsoft.EntityFrameworkCore，它包含很多好用的扩展方法，使用的AsyncQueryableExecuter + system.linq.dynamic 的方式
     * 而abp zero是直接引用的它
     * 可以考虑直接引用Microsoft.EntityFrameworkCore
     * 
     * 最后决定直接在应用层引入Microsoft.EntityFrameworkCore 因为这能提供及大的便利，且像AsNoChangeTracking无法用AsyncQueryableExecuter实现
     * 
     * InitializeOptionsAsync
     * asp.net userManager有个Option属性，abp的userManager会使用设置系统覆盖此设置 
     * 参考：https://github.com/aspnetboilerplate/aspnetboilerplate/blob/c0604b9b1347a3b9581bf97b4cae22db5b6bab1b/src/Abp.ZeroCore/Authorization/Users/AbpUserManager.cs
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
        protected readonly TRoleManager roleManager;
        protected readonly TUserManager userManager;
        protected readonly IRepository<CustomerEntity, long> repository;
        protected readonly IRepository<TUser, long> userRepository;
        protected readonly IRepository<AdministrativeEntity, long> administrativeRepository;

        public CustomerAppService(IRepository<CustomerEntity, long> repository,
                                  TRoleManager roleManager,
                                  TUserManager userManager,
                                  IRepository<TUser, long> userRepository,
                                  IRepository<AdministrativeEntity, long> administrativeRepository)
        {
            this.repository = repository;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.administrativeRepository = administrativeRepository;
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
            user.UserName = input.UserName ?? input.PhoneNumber ?? input.EmailAddress ?? BXJG.Common.SecurityHelper.RandomBase64() + Clock.Now.Ticks;

            //asp.net userManager有个Option属性，abp的userManager会使用设置系统覆盖此设置 参考：https://github.com/aspnetboilerplate/aspnetboilerplate/blob/c0604b9b1347a3b9581bf97b4cae22db5b6bab1b/src/Abp.ZeroCore/Authorization/Users/AbpUserManager.cs
            await userManager.InitializeOptionsAsync(AbpSession.TenantId);
            //user.SetNormalizedNames();//这个貌似没必要调了，UserManager内部会处理

            //abp 6.1.3未实施密码复杂性要求，而ChangePasswordAsync有验证，所以这里分开处理
            //参考：https://github.com/aspnetboilerplate/aspnetboilerplate/issues/6050
            CheckErrors(await userManager.CreateAsync(user/*, input.Password*/));
            if (input.Password.IsNullOrWhiteSpace())
                input.Password = BXJG.Common.SecurityHelper.RandomBase64(8);
            CheckErrors(await userManager.ChangePasswordAsync(user, input.Password));


            //目前不考虑多角色商城会员
            //if (input.RoleNames != null)
            //{
            //    CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            //}
            //BXJG.Common.SecurityHelper.RandomBase64
            CurrentUnitOfWork.SaveChanges();//保存后才能拿到新的UserId

            CheckErrors(await userManager.AddToRoleAsync(user, CoreConsts.CustomerRoleName));

            //return MapToEntityDto(user);
            #endregion

            //映射不太好处理，手动来吧
            var entity = new CustomerEntity(user.Id)
            {
                //Amount = input.Amount,
                Birthday = input.Birthday,
                AreaId = input.AreaId,
                Gender = input.Gender
                //Integral = input.Integral,
                //User = user,//下面设置了userId就行了

            };
            await repository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();//保存后才能拿到新的会员信息自增Id
            var r = await GetDtoAsync(entity.Id);
            r.Password = input.Password;
            return r;
        }

        public virtual async Task<CustomerDto> UpdateAsync(CustomerUpdateDto input)
        {
            var entity = await repository.GetAsync(input.Id);

            #region 更新主程序的用户信息
            var user = await userManager.GetUserByIdAsync(entity.UserId);
            user.EmailAddress = input.EmailAddress;
            user.Name = input.Name;
            user.Surname = user.Name;
            user.PhoneNumber = input.PhoneNumber;
            //user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;
            user.IsPhoneNumberConfirmed = true;
            user.UserName = input.UserName;
            user.IsActive = input.IsActive;

            //asp.net userManager有个Option属性，abp的userManager会使用设置系统覆盖此设置 参考：https://github.com/aspnetboilerplate/aspnetboilerplate/blob/c0604b9b1347a3b9581bf97b4cae22db5b6bab1b/src/Abp.ZeroCore/Authorization/Users/AbpUserManager.cs
            await userManager.InitializeOptionsAsync(AbpSession.TenantId);
            CheckErrors(await userManager.UpdateAsync(user));
            if (!input.Password.IsNullOrWhiteSpace())
                CheckErrors(await userManager.ChangePasswordAsync(user, input.Password));
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
            entity.AreaId = input.AreaId;
            //entity.Integral = input.Integral;
            //entity.TenantId = AbpSession.TenantId.Value;
            await repository.UpdateAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return await GetDtoAsync(input.Id);
        }

        public virtual async Task<PagedResultDto<CustomerDto>> GetAllAsync(GetAllCustomersInput input)
        {
            var query = from c in repository.GetAll()
                        join u1 in userRepository.GetAllIncluding(c => c.Roles) on c.UserId equals u1.Id into dc
                        from u in dc.DefaultIfEmpty()
                        join qy1 in administrativeRepository.GetAll() on c.AreaId equals qy1.Id into dc1
                        from area in dc1.DefaultIfEmpty()
                        select new { c, u, area };

            query = query.WhereIf(!input.Keywords.IsNullOrWhiteSpace(), c => c.u.Name.Contains(input.Keywords) || c.u.PhoneNumber.Contains(input.Keywords))
                         .WhereIf(!input.AreaCode.IsNullOrWhiteSpace(), c => c.area.Code.StartsWith(input.AreaCode));

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            if (!input.Sorting.IsNullOrWhiteSpace())
            {
                if (input.Sorting.Contains("FullName "))
                    input.Sorting = "u." + input.Sorting;
                else if (input.Sorting.Contains("areaDisplayName"))
                    input.Sorting = input.Sorting.Replace("area", "area.");
                else
                    input.Sorting = "c." + input.Sorting;
                query = query.OrderBy(input.Sorting);
            }

            query = query.PageBy(input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(c => MapToDto(c.c, c.u, c.area)).ToList();
            return new PagedResultDto<CustomerDto>(totalCount, dtos);
        }

        public virtual async Task<CustomerDto> GetAsync(EntityDto<long> input)
        {
            return await GetDtoAsync(input.Id);
        }

        public virtual async Task<BatchOperationResultLong> DeleteBatchAsync(BatchOperationInputLong input)
        {
            var result = new BatchOperationResultLong();
            var userIds = await base.AsyncQueryableExecuter.ToListAsync(repository.GetAll().Where(c => input.Ids.Contains(c.Id)).Select(c => new { c.Id, c.UserId }));
            var sss = userIds.Select(c => c.UserId);
            var users = await base.AsyncQueryableExecuter.ToListAsync(userRepository.GetAll().Where(c => sss.Contains(c.Id)));
            foreach (var item in input.Ids)
            {
                try
                {
                    await repository.DeleteAsync(item);
                    await userManager.DeleteAsync(users.Single(d => d.Id == userIds.Single(c => c.Id == item).UserId));
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
        protected virtual async Task<(CustomerEntity customer, TUser user, AdministrativeEntity area)> GetOneAsync(long id)
        {
            var query = from c in repository.GetAll().Where(c => c.Id == id)
                        join uu in userRepository.GetAllIncluding(c => c.Roles) on c.UserId equals uu.Id into dc
                        from u in dc.DefaultIfEmpty()
                        join qy1 in administrativeRepository.GetAll() on c.AreaId equals qy1.Id into dc1
                        from area in dc1.DefaultIfEmpty()
                        select new { c, u, area };
            var r = await AsyncQueryableExecuter.FirstOrDefaultAsync(query);
            return (r.c, r.u, r.area);
        }
        /// <summary>
        /// 将一对一的顾客实体和用户实体转换为顾客的查询模型
        /// </summary>
        /// <param name="c">顾客实体</param>
        /// <param name="u">用户实体</param>
        /// <param name="area">所属区域</param>
        /// <returns></returns>
        protected virtual CustomerDto MapToDto(CustomerEntity c, TUser u, AdministrativeEntity area = default)
        {
            var dto = ObjectMapper.Map<CustomerDto>(c);
            ObjectMapper.Map(u, dto);
            if (area != null)
                ObjectMapper.Map(area, dto);
            dto.Id = c.Id;
            dto.Password = string.Empty;
            return dto;
        }
        protected virtual async Task<CustomerDto> GetDtoAsync(long id)
        {
            var p = await GetOneAsync(id);
            return MapToDto(p.customer, p.user, p.area);
        }
    }
}

using Abp.Application.Services;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.MultiTenancy;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic;
using Abp.Extensions;
using BXJG.Shop.Customer.Dto;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using BXJG.Shop.Common;

namespace BXJG.Shop.Customer
{
    /*
     * 设计此功能时还没有引用Microsoft.EntityFrameworkCore，它包含很多好用的扩展方法，使用的AsyncQueryableExecuter + system.linq.dynamic 的方式
     * 而abp zero是直接引用的它
     * 可以考虑直接引用Microsoft.EntityFrameworkCore
     * 
     * 最后决定直接在应用层引入Microsoft.EntityFrameworkCore 因为这能提供及大的遍历，且像AsNoChangeTracking无法用AsyncQueryableExecuter实现
     * 
     * await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
     * 源码地址 https://github.com/aspnetboilerplate/aspnetboilerplate/blob/c0604b9b1347a3b9581bf97b4cae22db5b6bab1b/src/Abp.ZeroCore/Authorization/Users/AbpUserManager.cs
     * 默认项目模板中 用户新增和注册 都会调用此方法
     * 很费解，看源码 主要是从 abp Settings中获取值 然后赋值给 IdentityOptions选项对象
     * 但是这个选项对象要么是单例 要么是 一个请求一个实例，每次赋值是几个意思？
     * 暂时不纠结了
     */

    /// <summary>
    /// 商城会员（顾客）信息
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TTenantManager"></typeparam>
    /// <typeparam name="TUserManager"></typeparam>
    public class CustomerAppService<TTenant, TUser, TRole, TTenantManager, TUserManager,TArea>
        : BXJGShopAppServiceBase<TTenant, TUser, TRole, TTenantManager, TUserManager>, ICustomerAppService
        where TUser : AbpUser<TUser>, new()
        where TRole : AbpRole<TUser>, new()
        where TTenant : AbpTenant<TUser>
        where TTenantManager : AbpTenantManager<TTenant, TUser>
        where TUserManager : AbpUserManager<TRole, TUser>
        where TArea : GeneralTreeEntity<TArea>, IShopAdministrative
    {
        private readonly IRepository<CustomerEntity<TUser,TArea>, long> repository;
        public CustomerAppService(IRepository<CustomerEntity<TUser,TArea> ,long> repository)
        {
            this.repository = repository;
        }
        public async Task<CustomerDto> CreateAsync(CustomerUpdateDto input)
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

            //return MapToEntityDto(user);
            #endregion

            //映射不太好处理，手动来吧
            var entity = new CustomerEntity<TUser,TArea>
            {
                Amount = input.Amount,
                Birthday = input.Birthday,
                Gender = input.Gender,
                Integral = input.Integral,
                //User = user,//下面设置了userId就行了
                UserId = user.Id
            };
            await repository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();//保存后才能拿到新的会员信息自增Id
            return ObjectMapper.Map<CustomerDto>(entity);
        }

        public async Task<CustomerDto> UpdateAsync(CustomerUpdateDto input)
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



            entity.Amount = input.Amount;
            entity.Birthday = input.Birthday;
            entity.Gender = input.Gender;
            entity.Integral = input.Integral;
            //entity.TenantId = AbpSession.TenantId.Value;


            await repository.UpdateAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<CustomerDto>(entity);
        }

        public async Task<PagedResultDto<CustomerDto>> GetListAsync(GetAllCustomersInput input)
        {
            var query = repository.GetAllIncluding(c => c.User).AsNoTracking()
                .WhereIf(!input.Keywords.IsNullOrEmpty(), c => c.User.Name.Contains(input.Keywords) || c.User.PhoneNumber.Contains(input.Keywords));

            var totalCount = await query.CountAsync();

            if (!input.Sorting.IsNullOrWhiteSpace())
                query = query.OrderBy(input.Sorting);
            query = query.PageBy(input);

            var entities = await query.ToListAsync();

            return new PagedResultDto<CustomerDto>(
                totalCount,
                ObjectMapper.Map<IReadOnlyList<CustomerDto>>(entities)
            );
        }

        public async Task<long[]> DeleteAsync(params long[] ids)
        {
            var successIds = new List<long>();
            foreach (var item in ids)
            {
                try
                {
                    await repository.DeleteAsync(item);
                    successIds.Add(item);
                }
                catch (Exception ex)
                {
                    Logger.Warn("删除商城会员失败！", ex);
                }
            }
            return successIds.ToArray();
        }

        public async Task<CustomerDto> GetAsync(EntityDto<long> input)
        {
            var entity = await repository.GetAllIncluding(c => c.User).AsNoTracking().SingleAsync(c => c.Id == input.Id);
            return ObjectMapper.Map<CustomerDto>(entity);
        }
    }
}

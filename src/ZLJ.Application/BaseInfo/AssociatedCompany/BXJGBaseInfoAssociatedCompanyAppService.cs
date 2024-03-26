using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using ZLJ.Application.Admin.BaseInfo.AssociatedCompany.Dto;
using BXJG.Common.Dto;
using Microsoft.EntityFrameworkCore;
using ZLJ.Core.BaseInfo.AssociatedCompany;
//using ZLJ.Application.Admin.WorkOrder.RentOrderItem.Admin;
using Abp.Events.Bus.Entities;
using Microsoft.AspNetCore.Identity;
using ZLJ.Core.Customer;
using Abp.IdentityFramework;
using ZLJ.Core.Authorization.Users;
using ZLJ.Application.Admin.Authorization.Permissions;
using ZLJ.Application.Share.Authorization.Permissions;
using BXJG.Common.Contracts;

namespace ZLJ.Application.Admin.BaseInfo.AssociatedCompany
{
    /// <summary>
    /// 基础信息-来往单位
    /// </summary>
   // [Obsolete("此接口应移动到common项目中去")]
    [AbpAuthorize]
    public class BXJGBaseInfoAssociatedCompanyAppService : AdminCrudBaseAppService<AssociatedCompanyEntity,
                                                                               AssociatedCompanyDto,
                                                                               long,
                                                                               AssociatedCompanyGetAllInput,
                                                                               AssociatedCompanyEditDto>
    {
        public BXJGBaseInfoAssociatedCompanyAppService(IRepository<AssociatedCompanyEntity, long> repository) : base(repository)
        {
        }

        public  UserManager userManager { get; set; }
        public  IRepository<CustomerStaffInfoEntity, long> userRepository { get; set;  }

        protected override string CreatePermissionName => PermissionNames.BXJGBaseInfoAssociatedCompanyCreate;
        protected override string UpdatePermissionName => PermissionNames.BXJGBaseInfoAssociatedCompanyUpdate;
        protected override string DeletePermissionName => PermissionNames.BXJGBaseInfoAssociatedCompanyDelete;
        protected override string GetPermissionName => PermissionNames.BXJGBaseInfoAssociatedCompany;
        protected override string GetAllPermissionName => PermissionNames.BXJGBaseInfoAssociatedCompany;


        public override async Task<AssociatedCompanyDto> CreateAsync(AssociatedCompanyEditDto input)
        {
            //var existsQuery = Repository.GetAll().Where(x => x.Name == input.Name && input.Id != x.Id);
            //if (await AsyncQueryableExecuter.AnyAsync(existsQuery))
            //    throw new UserFriendlyException("名称已存在");
            var dto = await base.CreateAsync(input);
            //若是客户，新增管理员账号
            if (dto.CategoryId != 17)
            {
                dto.AdminUserName = input.LinkPhone ?? TinyPinyin.PinyinHelper.GetPinyinInitials(input.LinkMan) + dto.Id;
                dto.AdminEquipmentPwd = input.AdminEquipmentPwd;// SecurityHelper.RandomBase64();
                var entity = new CustomerStaffInfoEntity
                {
                    //AccessFailedCount = 0,
                    CustomerId = dto.Id,
                    UserName = dto.AdminUserName, //应由创建者提供
                    Surname = input.Name,
                    Pinyin = TinyPinyin.PinyinHelper.GetPinyinInitials(input.Name),
                    PhoneNumber = input.LinkPhone,
                    Name = input.LinkMan,
                    Gender = Gender.Man,
                    // FullName = input.LinkMan,
                    // NormalizedUserName = 
                    EmailAddress = Guid.NewGuid().ToString("n") + "@a.com",
                    IsEmailConfirmed = true,
                    IsActive = input.IsActive,
                    IsTwoFactorEnabled = false,
                    IsPhoneNumberConfirmed = true,
                    IsLockoutEnabled = true,
                    EquipmentPwd = dto.AdminEquipmentPwd
                };
                await userManager.InitializeOptionsAsync(AbpSession.TenantId);
                CheckErrors(await userManager.CreateAsync(entity, dto.AdminEquipmentPwd));
                CheckErrors(await userManager.AddToRoleAsync(entity, CustomerRole.CustomerAdminRole));
                entity.Customer.AdminId = entity.Id;
                //await userManager.SetOrganizationUnitsAsync(entity.Id, new[] { input.OuId });
                //var ou = ouRepository.Get(input.OuId);
                //CurrentUnitOfWork.Items["ous"] = new Dictionary<long, CustomerOUEntity> { { entity.Id, ou } };
                //return MapToEntityDto(entity);
            }
            return dto;
        }
        protected override AssociatedCompanyEntity MapToEntity(AssociatedCompanyEditDto createInput)
        {
            var r = base.MapToEntity(createInput);
            r.Pinyin = TinyPinyin.PinyinHelper.GetPinyinInitials(createInput.Name);
            return r;
        }
        protected override void MapToEntity(AssociatedCompanyEditDto updateInput, AssociatedCompanyEntity entity)
        {
            base.MapToEntity(updateInput, entity);

            entity.Pinyin = TinyPinyin.PinyinHelper.GetPinyinInitials(updateInput.Name);
        }
        public override async Task<AssociatedCompanyDto> UpdateAsync(AssociatedCompanyEditDto input)
        {
            CheckUpdatePermission();
            var entity = await GetEntityByIdAsync(input.Id);
            MapToEntity(input, entity);
            await userManager.InitializeOptionsAsync(AbpSession.TenantId);
            await userManager.ChangePasswordAsync(entity.Admin, input.AdminEquipmentPwd);
            entity.Admin.EquipmentPwd = input.AdminEquipmentPwd;
            await base.CurrentUnitOfWork.SaveChangesAsync();
            return MapToEntityDto(entity);

            // var entiy = Repository.GetAll().Include(c=>c.Admin).Where(x => x.Name == input.Name && input.Id != x.Id);




            //  return await base.UpdateAsync(input);
        }

        protected override async ValueTask BatchDeleteItemAsync(AssociatedCompanyEntity u)
        {
            if (u.CategoryId != 17)
            {
                //新增的客户，没有添加其它信息时删除管理员，若存在其它的，则外键会约束
                var statffs = userRepository.GetAll().Where(c => c.CustomerId == u.Id);
                foreach (var custStaff in statffs)
                {
                    if (await userManager.IsInRoleAsync(custStaff, CustomerRole.CustomerAdminRole))
                    {
                        await userManager.DeleteAsync(custStaff);
                    }
                }
            }
          //  await Repository.DeleteAsync(u);
            await base.BatchDeleteItemAsync(u);
        }
        ///// <summary>
        ///// 批量删除
        ///// </summary>
        //public async Task<BatchOperationOutputLong> DeleteBatchAsync(BatchOperationInputLong input)
        //{
        //    CheckDeletePermission();
        //    var result = new BatchOperationOutputLong();

        //    foreach (var item in input.Ids)
        //    {
        //        try
        //        {
        //            using var sw = base.UnitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew);
        //            var u = await Repository.GetAsync(item);

        //            if (u.CategoryId != 17)
        //            {
        //                //新增的客户，没有添加其它信息时删除管理员，若存在其它的，则外键会约束
        //                var statffs = userRepository.GetAll().Where(c => c.CustomerId == item);
        //                foreach (var custStaff in statffs)
        //                {
        //                    if (await userManager.IsInRoleAsync(custStaff, CustomerRole.CustomerAdminRole))
        //                    {
        //                        await userManager.DeleteAsync(custStaff);
        //                    }
        //                }
        //            }
        //            await Repository.DeleteAsync(u);
        //            result.Ids.Add(item);

        //            //userManager.role.GetUsersInRoleAsync(CustomerRole.CustomerAdminRole);

        //            await sw.CompleteAsync();
        //        }
        //        catch (UserFriendlyException ex)
        //        {
        //            result.ErrorMessage.Add(new BatchOperationErrorMessage(item, ex.Message));
        //        }
        //        catch (Exception ex)
        //        {
        //            result.ErrorMessage.Add(item.Message500());
        //            Logger.Warn($"删除来往单位失败，Id：{item}", ex);
        //        }
        //    }

        //    return result;
        //}

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            CheckDeletePermission();
            var u = await Repository.GetAsync(input.Id);

            if (u.CategoryId != 17)
            {
                //新增的客户，没有添加其它信息时删除管理员，若存在其它的，则外键会约束
                var statffs = userRepository.GetAll().Where(c => c.CustomerId == input.Id);
                foreach (var custStaff in statffs)
                {
                    if (await userManager.IsInRoleAsync(custStaff, CustomerRole.CustomerAdminRole))
                    {
                        await userManager.DeleteAsync(custStaff);
                    }
                }
            }
            await base.DeleteAsync(input);
        }

        /// <summary>
        /// CreateFilteredQuery
        /// </summary>
        protected override IQueryable<AssociatedCompanyEntity> CreateFilteredQuery(AssociatedCompanyGetAllInput input)
        {
            return base.CreateFilteredQuery(input)
                .Include(c => c.Area)
                .Include(x => x.Level)
                .Include(c => c.Admin)
                .Include(x => x.Category)
                //.WhereIf(!input.Q.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Q))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive)
                .WhereIf(input.LevelId.HasValue, x => x.LevelId == input.LevelId)
                .WhereIf(input.CategoryId.HasValue, x => x.CategoryId == input.CategoryId)
                .WhereIf(!input.AreaCode.IsNullOrWhiteSpace(), c => c.Area.Code.StartsWith(input.AreaCode))
                .WhereIf(!input.Keywords.IsNullOrWhiteSpace(), c => c.Name.Contains(input.Keywords) ||
                                                                    c.TaxNo.Contains(input.Keywords) ||
                                                                    c.Pinyin.Contains(input.Keywords));
        }

        protected override Task<AssociatedCompanyEntity> GetEntityByIdAsync(long id)
        {
            return Repository.GetAll().Include(c => c.Area)
                .Include(x => x.Level)
                .Include(c => c.Admin)
                .Include(x => x.Category).Where(c => c.Id == id).SingleAsync();
        }

        //public override Task<PagedResultDto<AssociatedCompanyDto>> GetAllAsync(AssociatedCompanyGetAllInput input)
        //{
        //    return base.GetAllAsync(input);
        //}

        //protected override Task<AssociatedCompanyEntity> GetEntityByIdAsync(long id)
        //{
        //    return base.GetEntityByIdAsync(id);
        //}

        //protected override AssociatedCompanyDto MapToEntityDto(AssociatedCompanyEntity entity)
        //{
        //    var dto = base.MapToEntityDto(entity);
        //    var dic = CurrentUnitOfWork.Items["admins"] as Dictionary<long, CustomerStaffInfoEntity>;
        //    if (dic.TryGetValue(entity.Id, out var admin))
        //    {
        //        dto.LoginName = admin.UserName;
        //        dto.Password = admin.EquipmentPwd;
        //    }
        //    return dto;
        //}

        //override getall

        void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(base.LocalizationManager);
        }
    }
}
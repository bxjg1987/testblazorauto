using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;

using Microsoft.EntityFrameworkCore;
//using ZLJ.Application.WorkOrder.RentOrderItem.Admin;
using Abp.Events.Bus.Entities;
using Microsoft.AspNetCore.Identity;
using ZLJ.Core.Customer;
using Abp.IdentityFramework;
using ZLJ.Core.Authorization.Users;
using ZLJ.Application.Authorization.Permissions;
using ZLJ.Application.Share.Authorization.Permissions;
using ZLJ.Core.AssociatedCompany;
using BXJG.Common.Contracts;
using ZLJ.Application.Share.AssociatedCompany;
using ZLJ.Application;
using Abp.Notifications;

namespace ZLJ.Application.AssociatedCompany
{
    /// <summary>
    /// 基础信息-来往单位
    /// </summary>
    [AbpAuthorize]
    public class AssociatedCompanyAppService : AdminCrudBaseAppService<AssociatedCompanyEntity,
                                                                       AssociatedCompanyDto,
                                                                       long,
                                                                      PagedAndSortedResultRequest<  AssociatedCompanyCondition>,
                                                                       AssociatedCompanyEditDto>
    {

        public AssociatedCompanyAppService(IRepository<AssociatedCompanyEntity, long> repository) : base(repository)
        {
        }

        //public UserManager userManager { get; set; }
        //public IRepository<CustomerStaffInfoEntity, long> userRepository { get; set; }

        protected override string CreatePermissionName => PermissionNames.BXJGBaseInfoAssociatedCompanyCreate;
        protected override string UpdatePermissionName => PermissionNames.BXJGBaseInfoAssociatedCompanyUpdate;
        protected override string DeletePermissionName => PermissionNames.BXJGBaseInfoAssociatedCompanyDelete;
        protected override string GetPermissionName => PermissionNames.BXJGBaseInfoAssociatedCompany;
        protected override string GetAllPermissionName => PermissionNames.BXJGBaseInfoAssociatedCompany;

        public INotificationPublisher  Notification { get; set; }

        public override async Task<AssociatedCompanyDto> CreateAsync(AssociatedCompanyEditDto input)
        {
            await Repository.IsExistsThrow(x => x.Name == input.Name,x=>x.Name,CancellationTokenProvider.Token);
            return await base.CreateAsync(input);
        }

        public override async Task<AssociatedCompanyDto> UpdateAsync(AssociatedCompanyEditDto input)
        {
          //await  Notification.PublishAsync("通知名称",
          //      new MessageNotificationData( "测试通知"+DateTime.Now.ToLongDateString()), 
          //      userIds: [new UserIdentifier(base.AbpSession.TenantId, base.AbpSession.UserId.Value)]);

            await Repository.IsExistsThrow(x => x.Name == input.Name&& x.Id!=input.Id, x => x.Name, CancellationTokenProvider.Token);

            return await base.UpdateAsync(input);
        }

        protected override async ValueTask MapToEntity(AssociatedCompanyEntity entity)
        {
           // /最好是前端判断，后端兜底
            await  base.MapToEntity(entity);
            entity.Pinyin = TinyPinyin.PinyinHelper.GetPinyinInitials(entity.Name);
            entity.AddressPinyin = entity.Address.IsNotNullOrWhiteSpaceBXJG() ? TinyPinyin.PinyinHelper.GetPinyinInitials(entity.Address) : default;
            entity.LinkManPinyin = entity.LinkMan.IsNotNullOrWhiteSpaceBXJG() ? TinyPinyin.PinyinHelper.GetPinyinInitials(entity.LinkMan) : default;
        }
        //public override async Task<AssociatedCompanyDto> UpdateAsync(AssociatedCompanyEditDto input)
        //{
        //    CheckUpdatePermission();
        //    var entity = await GetEntityByIdAsync(input.Id);
        //    MapToEntity(input, entity);
        //    //await userManager.InitializeOptionsAsync(AbpSession.TenantId);
        //    //await userManager.ChangePasswordAsync(entity.Admin, input.AdminEquipmentPwd);
        //    //entity.Admin.EquipmentPwd = input.AdminEquipmentPwd;
        //    await CurrentUnitOfWork.SaveChangesAsync();
        //    return MapToEntityDto(entity);

        //    // var entiy = Repository.GetAll().Include(c=>c.Admin).Where(x => x.Name == input.Name && input.Id != x.Id);




        //    //  return await base.UpdateAsync(input);
        //}

        //protected override async ValueTask BatchDeleteItemAsync(AssociatedCompanyEntity u)
        //{
        //    if (u.CategoryId != 17)
        //    {
        //        //新增的客户，没有添加其它信息时删除管理员，若存在其它的，则外键会约束
        //        var statffs = userRepository.GetAll().Where(c => c.CustomerId == u.Id);
        //        foreach (var custStaff in statffs)
        //        {
        //            if (await userManager.IsInRoleAsync(custStaff, CustomerRole.CustomerAdminRole))
        //            {
        //                await userManager.DeleteAsync(custStaff);
        //            }
        //        }
        //    }
        //  //  await Repository.DeleteAsync(u);
        //    await base.BatchDeleteItemAsync(u);
        //}
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

        //public override async Task DeleteAsync(EntityDto<long> input)
        //{
        //    CheckDeletePermission();
        //    var u = await Repository.GetAsync(input.Id);

        //    if (u.CategoryId != 17)
        //    {
        //        //新增的客户，没有添加其它信息时删除管理员，若存在其它的，则外键会约束
        //        var statffs = userRepository.GetAll().Where(c => c.CustomerId == input.Id);
        //        foreach (var custStaff in statffs)
        //        {
        //            if (await userManager.IsInRoleAsync(custStaff, CustomerRole.CustomerAdminRole))
        //            {
        //                await userManager.DeleteAsync(custStaff);
        //            }
        //        }
        //    }
        //    await base.DeleteAsync(input);
        //}

        /// <summary>
        /// CreateFilteredQuery
        /// </summary>
        protected override async Task<IQueryable<AssociatedCompanyEntity>> CreateFilteredQuery(PagedAndSortedResultRequest<AssociatedCompanyCondition> input)
        {
            return (await base.CreateFilteredQuery(input))
                //.Include(c => c.Area)
                //.Include(x => x.Level)
                //.Include(c => c.Admin)
                //.Include(x => x.Category)
                //.WhereIf(!input.Q.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Q))
                .WhereIf(input.Filter.IsActive.HasValue, x => x.IsActive == input.Filter.IsActive)
                .WhereIf(input.Filter.LevelId.HasValue, x => x.LevelId == input.Filter.LevelId)
                //.WhereIf(input.CategoryId.HasValue, x => x.CategoryId == input.CategoryId)
                .WhereIf(input.Filter.AreaCode.IsNotNullOrWhiteSpaceBXJG(), c => c.Area.Code.StartsWith(input.Filter.AreaCode))
                .WhereIf(input.Filter.Keywords.IsNotNullOrWhiteSpaceBXJG(), c => c.Name.Contains(input.Filter.Keywords) ||
                                                                    c.TaxNo.Contains(input.Filter.Keywords) ||
                                                                    c.Pinyin.Contains(input.Filter.Keywords) ||
                                                                    c.LinkMan.Contains(input.Filter.Keywords) ||
                                                                    c.LinkManPinyin.Contains(input.Filter.Keywords) ||
                                                                    c.LinkPhone.Contains(input.Filter.Keywords) ||
                                                                    c.AddressPinyin.Contains(input.Filter.Keywords) ||
                                                                    c.Address.Contains(input.Filter.Keywords));
        }

        //protected override Task<AssociatedCompanyEntity> GetEntityByIdAsync(long id)
        //{
        //    return Repository.GetAll().Include(c => c.Area)
        //        .Include(x => x.Level)
        //        //.Include(c => c.Admin)
        //        //.Include(x => x.Category)
        //        .Where(c => c.Id == id).SingleAsync();
        //}
        //override app
        protected override async Task<IQueryable<AssociatedCompanyEntity>> BuildQuery(bool track=true)
        {
            return (await base.BuildQuery(track)).Include(x=>x.Area).Include(x=>x.Level);
        }

        protected override IQueryable<AssociatedCompanyEntity> ApplySorting(IQueryable<AssociatedCompanyEntity> query, PagedAndSortedResultRequest<AssociatedCompanyCondition> input)
        {
            input.Sorting = input.Sorting.Replace("LevelDisplayName", "Level.DisplayName").Replace("AreaDisplayName", "Area.DisplayName");
            return base.ApplySorting(query, input);
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

        //void CheckErrors(IdentityResult identityResult)
        //{
        //    identityResult.CheckErrors(LocalizationManager);
        //}
    }
}
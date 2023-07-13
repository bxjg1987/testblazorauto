using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Organizations;
using BXJG.Common.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.App.Customer.Authorization.Permissions;
using ZLJ.BaseInfo;
using ZLJ.Customer;

namespace ZLJ.App.Customer.OU
{
    //[AbpAuthorize(Auth.PermissionNames.OU)]
    //[AbpAuthorize(PermissionNames.Customer)]

    /// <summary>
    /// 客户管理员对客户部门进行管理的接口
    /// </summary>
    [AbpAuthorize(PermissionNames.Customer)]
    public class OuAppService : CustomerBaseAppService
    {
        public CustomerOUManager OrganizationUnitManager { get; private set; }
        IRepository<CustomerOUEntity, long> repository;
        public OuAppService(IRepository<CustomerOUEntity, long> repository, CustomerOUManager organizationUnitManager)// : base(repository)
        {
            //base.LocalizationManager.sou
            //LocalizationSourceName = ZLJConsts.LocalizationSourceName;
            this.repository = repository;
            this.OrganizationUnitManager = organizationUnitManager;
        }
        // [AbpAuthorize(Auth.PermissionNames.OUCreate)]
        public virtual async Task<OUDto> CreateAsync(OUEditDto input)
        {
            var ou = new CustomerOUEntity();
            ou.ParentId = input.ParentId;
            ou.DisplayName = input.DisplayName;
            ou.IsActive = input.IsActive;
            ou.CustomerId = CustomerSession.CustomerId.Value;
            await OrganizationUnitManager.CreateAsync(ou);
            await CurrentUnitOfWork.SaveChangesAsync();
            return await Map2Dto(ou);
        }

        //  [AbpAuthorize(Auth.PermissionNames.OUUpdate)]
        public virtual async Task<OUDto> UpdateAsync(OUEditDto input)
        {
            var ou = await repository.GetAsync(input.Id);
            ou.ParentId = input.ParentId;
            ou.DisplayName = input.DisplayName;

            ou.IsActive = input.IsActive;
            await OrganizationUnitManager.UpdateAsync(ou);
            //await organizationUnitManager.CreateAsync(ou);
            await CurrentUnitOfWork.SaveChangesAsync();
            return await Map2Dto(ou);
        }
        //  [AbpAuthorize(Auth.PermissionNames.OUUpdate)]
        public virtual async Task MoveAsync(BXJG.Utils.GeneralTree.GeneralTreeNodeMoveInput input)
        {
            await this.OrganizationUnitManager.MoveAsync(input.Id, input.TargetId);
        }

        //[AbpAuthorize(Auth.PermissionNames.OUDelete)]
        public virtual async Task<BatchOperationOutputLong> DeleteAsync(BatchOperationInputLong input)
        {
            var r = new BatchOperationOutputLong();
            foreach (var item in input.Ids)
            {
                try
                {
                    var sw = UnitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew);
                    await repository.DeleteAsync(item);
                    await sw.CompleteAsync();
                    r.Ids.Add(item);
                }
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(item));
                }
                catch (Exception)
                {
                    r.ErrorMessage.Add(item.Message500());
                }
            }
            return r;
        }
        [UnitOfWork(false)]
        public virtual async Task<List<OUDto>> GetAllAsync(GetAllInput input)
        {
            string code = "";
            if (input.ParentId.HasValue && input.ParentId.Value > 0)
            {
                code = await repository.GetAll().Where(c => c.Id == input.ParentId).Select(c => c.Code).SingleAsync();
            }
            else
                code = input.ParentCode;

            var q = repository.GetAll().OfType<ZLJ.Customer.CustomerOUEntity>().AsNoTrackingWithIdentityResolution()
                                //ef全局过滤器不支持继承的实体，手动来吧
                                .Where(c => c.CustomerId == CustomerSession.CustomerId)
                                .WhereIf(!code.IsNullOrWhiteSpace(), c => c.Code.StartsWith(code))
                                .WhereIf(input.IsActive.HasValue, c => c.IsActive == input.IsActive.Value)
                                .WhereIf(!input.Keywords.IsNullOrWhiteSpace(), c => c.DisplayName.Contains(input.Keywords))
                                .WhereIf(!input.LoadParent || !input.ParentText.IsNullOrWhiteSpace(), c => c.Code != code);
            q = q.OrderBy(c => c.Code);
            var list = await q.ToListAsync();
            var dtos = new List<OUDto>();
            foreach (var item in list)
            {
                var dto = await Map2Dto(item);
                dtos.Add(dto);
            }
            dtos.ForEach(c => c.Children = dtos.Where(d => d.ParentId == c.Id).ToList());

            var p = list.SingleOrDefault(c => c.Code == code);
            if (!input.LoadParent)
            {
                if (p == default)
                    return dtos.Where(c => !c.ParentId.HasValue).ToList();
                return dtos.Where(c => c.ParentId == p.Id).ToList();
            }
            var pDto = new OUDto();// {  DisplayName = "" };
            if (input.ParentText.IsNullOrWhiteSpace())
                pDto.DisplayName = "全部";
            else
                pDto.DisplayName = input.ParentText;
            dtos.Insert(0, pDto);

            return dtos;
        }

        [UnitOfWork(false)]
        public virtual async Task<OUDto> GetAsync(EntityDto<long> input)
        {
            var entity = await repository.GetAll().AsNoTrackingWithIdentityResolution().SingleAsync(c => c.Id == input.Id && c.CustomerId == CustomerSession.CustomerId);
            return await Map2Dto(entity);
        }

        protected ValueTask<OUDto> Map2Dto(CustomerOUEntity entity)
        {
            var dto = new OUDto()
            {
                Code = entity.Code,
                CreationTime = entity.CreationTime,
                CreatorUserId = entity.CreatorUserId,
                DisplayName = entity.DisplayName,
                Id = entity.Id,
                IsActive = entity.IsActive,
                LastModificationTime = entity.LastModificationTime,
                LastModifierUserId = entity.LastModifierUserId,
                ParentId = entity.ParentId,
            };
            return new ValueTask<OUDto>(dto);
        }
        /// <summary>
        /// 开关
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<BatchOperationOutputLong> SwitchActive(BatchSwitchInputLong input)
        {
            var result = new BatchOperationOutputLong();
            foreach (var id in input.Ids)
            {
                try
                {
                    var sw = UnitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew);
                    var item = await repository.SingleAsync(c => c.Id == id);
                    item.IsActive = input.IsActive;
                    await sw.CompleteAsync();
                    result.Ids.Add(item.Id);
                }
                catch (UserFriendlyException ex)
                {
                    result.ErrorMessage.Add(new BatchOperationErrorMessage(id, ex.Message));
                }
                catch (Exception ex)
                {
                    result.ErrorMessage.Add(BatchOperationErrorMessageExt.Message500(id));
                    Logger.Warn($"部分或全部开关失败，客户部门Id：{id}", ex);
                }
            }
            return result;
        }
    }
}

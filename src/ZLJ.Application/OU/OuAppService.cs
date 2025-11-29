using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Organizations;
using Abp.Threading;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.OU;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Authorization.Permissions;
using ZLJ.Application.BaseInfo;
using ZLJ.Application.Share;
using ZLJ.Application.Share.Authorization.Permissions;
using ZLJ.Application.Share.OU;
using ZLJ.Core.Localization;
using ZLJ.Core.OU;

namespace ZLJ.Application.OU
{
    [AbpAuthorize(PermissionNames.AdministratorBaseInfoOrganizationUnitGet)]
    public class OuAppService : AdminBaseAppService//<OrganizationUnit, OUDto, long, GetAllInput, OUEditDto>
    {
        BXJGOrganizationUnitManager organizationUnitManager;
        IRepository<OrganizationUnit, long> repository;
        public OuAppService(IRepository<OrganizationUnit, long> repository, BXJGOrganizationUnitManager organizationUnitManager)// : base(repository)
        {
            //LocalizationSourceName = AdminConsts.Admin;
            this.repository = repository;
            this.organizationUnitManager = organizationUnitManager;
        }
        [AbpAuthorize(PermissionNames.AdministratorBaseInfoOrganizationUnitAdd)]
        public async Task<OUDto> CreateAsync(OUCreateDto input)
        {
            var ou = new OrganizationUnitEntity();
            ou.ParentId = input.ParentId;
            ou.DisplayName = input.DisplayName;
            ou.OUType = input.OUType;
            ou.TenantId = AbpSession.TenantId;
            await organizationUnitManager.CreateAsync(ou);
            await CurrentUnitOfWork.SaveChangesAsync();
            return await Map2Dto(ou);
        }

        [AbpAuthorize(PermissionNames.AdministratorBaseInfoOrganizationUnitUpdate)]
        public async Task<OUDto> UpdateAsync(OUEditDto input)
        {
            var ou = await (await repository.GetAllAsync()).OfType<OrganizationUnitEntity>().Include(x => x.Parent).FirstAsync(x => x.Id == input.Id, CancellationTokenProvider.Token);
            //ou.ParentId = input.ParentId;
            ou.DisplayName = input.DisplayName;

            //var ou2 = ou as OrganizationUnitEntity;
            ou.OUType = input.OUType;


            //await organizationUnitManager.CreateAsync(ou);
            // 
            await organizationUnitManager.UpdateAsync(ou);
            await CurrentUnitOfWork.SaveChangesAsync();
            if (input.ParentId == 0)
                input.ParentId = default;
            if (input.ParentId != ou.ParentId)
            {
                await organizationUnitManager.MoveAsync(ou.Id, input.ParentId);
                await UpdateChildrenCount(ou.Parent);
            }



            return await Map2Dto(ou);
        }
        [AbpAuthorize(PermissionNames.AdministratorBaseInfoOrganizationUnitUpdate)]
        public async Task MoveAsync(GeneralTreeNodeMoveInput input)
        {
            await organizationUnitManager.MoveAsync(input.Id, input.TargetId);
        }

        [AbpAuthorize(PermissionNames.AdministratorBaseInfoOrganizationUnitDelete)]
        public async Task<BatchOperationOutputLong> DeleteBatchAsync(BatchOperationInputLong input)
        {
            var r = new BatchOperationOutputLong();
            foreach (var item in input.Ids)
            {
                try
                {
                    var sw = UnitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew);
                    await DeleteCore(item);
                    await sw.CompleteAsync();
                    r.Ids.Add(item);
                }
                catch (Exception)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(item));
                }
            }
            return r;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.AdministratorBaseInfoOrganizationUnitDelete)]
        public async Task DeleteAsync(EntityDto<long> input)
        {
            await DeleteCore(input.Id);
        }
        private async Task DeleteCore(long item)
        {
            var entity = await (await repository.GetAllAsync()).Include(x => x.Children).Include(x => x.Parent).Where(x => x.Id == item).FirstAsync(CancellationTokenProvider.Token);
            //await repository.DeleteAsync(entity);
            await organizationUnitManager.DeleteAsync(item);
            //await CurrentUnitOfWork.SaveChangesAsync();
            //if (entity.Parent != null)
            //    await UpdateChildrenCount(entity.Parent);

        }


        [UnitOfWork(false)]
        public async Task<IList<OUDto>> GetAllAsync(GetAllInput input)
        {
            string code = string.Empty;
            if (input.ParentId.HasValue && input.ParentId.Value > 0)
            {
                code = await (await repository.GetAllAsync()).Where(c => c.Id == input.ParentId).Select(c => c.Code).SingleAsync(CancellationTokenProvider.Token);
            }
            else
                code = input.ParentCode;

            var q = (await repository.GetAllAsync())
                              .OfType<OrganizationUnitEntity>()
                              .AsNoTrackingWithIdentityResolution()
                              .Include(c => c.Children).Include(c => c.Parent)
                              .WhereIf(!code.IsNullOrWhiteSpace(), c => c.Code.StartsWith(code));
            q = q.OrderBy(c => c.Code);
            var list = await q.ToListAsync(CancellationTokenProvider.Token);
            if (!input.ParentId.HasValue)
                input.ParentId = list.OrderBy(c => c.Code.Length).FirstOrDefault()?.ParentId;
            var dtos = new List<OUDto>();
            foreach (var item in list)
            {
                var dto = await Map2Dto(item);
                dtos.Add(dto);
            }
            dtos.ForEach(c => c.Children = dtos.Where(d => d.ParentId == c.Id).ToList());



            return dtos.Where(x => x.ParentId == input.ParentId).ToList();
        }

        [UnitOfWork(false)]
        public async Task<OUDto> GetAsync(EntityDto<long> input)
        {
            var entity = await (await repository.GetAllAsync())
                              //.OfType<OrganizationUnitEntity>()
                              .AsNoTrackingWithIdentityResolution().Include(c => c.Parent)
                              .Include(c => c.Children).SingleAsync(c => c.Id == input.Id);
            return await Map2Dto(entity);
        }

        protected virtual ValueTask<OUDto> Map2Dto(OrganizationUnit entity)
        {
            var item = entity as OrganizationUnitEntity;
            var dto = new OUDto();
            dto.Code = entity.Code;
            dto.DisplayName = entity.DisplayName;
            dto.Id = entity.Id;
            dto.ParentId = entity.ParentId;
            dto.OUType = item.OUType;
            dto.OuTypeText = item.OUType.Enum(); //base.L() item.OUTypeText;
            dto.ParentDisplayName = entity.Parent?.DisplayName;
            dto.ChildrenCount = item.ChildrenCount;

            return new ValueTask<OUDto>(dto);
        }

        /// <summary>
        /// 更新节点的子节点数量
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        async Task UpdateChildrenCount(OrganizationUnit entity)
        {
            (entity as OrganizationUnitEntity).ChildrenCount = await AsyncQueryableExecuter.CountAsync((await repository.GetAllReadonlyAsync()).Where(c => c.ParentId == entity.Id));
        }
    }
}

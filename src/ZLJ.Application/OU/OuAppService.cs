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
using ZLJ.App.Admin.Authorization.Permissions;
using ZLJ.App.Admin.BaseInfo;
using ZLJ.BaseInfo;

namespace ZLJ.App.Admin.OU
{
    [AbpAuthorize(PermissionNames.AdministratorBaseInfoOrganizationUnit)]
    public class OuAppService : ApplicationService//<OrganizationUnit, OUDto, long, GetAllInput, OUEditDto>
    {
        OrganizationUnitManager organizationUnitManager;
        IRepository<OrganizationUnit, long> repository;
        public OuAppService(IRepository<OrganizationUnit, long> repository, OrganizationUnitManager organizationUnitManager)// : base(repository)
        {
            LocalizationSourceName = ZLJConsts.LocalizationSourceName;
            this.repository = repository;
            this.organizationUnitManager = organizationUnitManager;
        }
        [AbpAuthorize(PermissionNames.AdministratorBaseInfoOrganizationUnitAdd)]
        public async Task<OUDto> CreateAsync(OUEditDto input)
        {
            var ou = new OrganizationUnitEntity();
            ou.ParentId = input.ParentId;
            ou.DisplayName = input.DisplayName;
            ou.OUType = input.OUType;
            await organizationUnitManager.CreateAsync(ou);
            await CurrentUnitOfWork.SaveChangesAsync();
            return await Map2Dto(ou);
        }

        [AbpAuthorize(PermissionNames.AdministratorBaseInfoOrganizationUnitUpdate)]
        public async Task<OUDto> UpdateAsync(OUEditDto input)
        {
            var ou = await repository.GetAsync(input.Id);
            ou.ParentId = input.ParentId;
            ou.DisplayName = input.DisplayName;
            //await organizationUnitManager.CreateAsync(ou);
            await CurrentUnitOfWork.SaveChangesAsync();
            return await Map2Dto(ou);
        }
        [AbpAuthorize(PermissionNames.AdministratorBaseInfoOrganizationUnitUpdate)]
        public async Task MoveAsync(BXJG.Utils.GeneralTree.GeneralTreeNodeMoveInput input)
        {
            await this.organizationUnitManager.MoveAsync(input.Id, input.TargetId);
        }

        [AbpAuthorize(PermissionNames.AdministratorBaseInfoOrganizationUnitDelete)]
        public async Task<BatchOperationOutputLong> DeleteAsync(BatchOperationInputLong input)
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
                catch (Exception)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(item));
                }
            }
            return r;
        }

        [UnitOfWork(false)]
        public async Task<IList<OUDto>> GetAllAsync(GetAllInput input)
        {
            string code = "";
            if (input.ParentId.HasValue && input.ParentId.Value > 0)
            {
                code = await repository.GetAll().Where(c => c.Id == input.ParentId).Select(c => c.Code).SingleAsync();
            }
            else
                code = input.ParentCode;

            var q = repository.GetAll()
                              .OfType<OrganizationUnitEntity>()
                              .AsNoTrackingWithIdentityResolution()
                              .Include(c => c.Children)
                              .WhereIf(!code.IsNullOrWhiteSpace(), c => c.Code.StartsWith(code))
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
        public async Task<OUDto> GetAsync(EntityDto<long> input)
        {
            var entity = await repository.GetAll()
                              //.OfType<OrganizationUnitEntity>()
                              .AsNoTrackingWithIdentityResolution()
                              .Include(c => c.Children).SingleAsync(c=>c.Id==input.Id);
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
            return new ValueTask<OUDto>(dto);
        }
    }
}

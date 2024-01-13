using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Organizations;
using BXJG.Utils.Application.Share.GeneralTree;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.OU;
using ZLJ.Core.BaseInfo;
using ZLJ.Core.Share;

namespace ZLJ.Application.Common.OU
{
    /// <summary>
    /// 提供公司、部门下拉树形数据；登陆用户即可访问，将来增加权限依赖
    /// </summary>
    [AbpAuthorize]
    [UnitOfWork(false)]
    public class OuProviderAppService : CommonBaseAppService, IOuProviderAppService
    {
        IRepository<OrganizationUnit, long> repository;

        public OuProviderAppService(IRepository<OrganizationUnit, long> repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// 提供公司、部门下拉树形数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
     
        public async Task<IList<OuDto>> GetTreeForSelectAsync(GetListInput input)
        {
            if (input.Code.IsNullOrWhiteSpace() && input.ParentId.HasValue && input.ParentId.Value > 0)
                input.Code = await repository.GetAll().Where(c => c.Id == input.ParentId).Select(c => c.Code).SingleAsync();

            var q = repository.GetAll();

            if (input.WhatType == 0)
                q = q.OfType<OrganizationUnitEntity>();
            else
                q = q.OfType<ZLJ.Core.Customer.CustomerOUEntity>();
            q = q.AsNoTrackingWithIdentityResolution();

            if (input.IsOnlyLoadChild)
            {
                q = q.WhereIf(input.Code.IsNotNullOrWhiteSpaceBXJG(), c => c.Parent.Code == input.Code)
                     .WhereIf(input.Code.IsNullOrWhiteSpaceBXJG(), c => !c.ParentId.HasValue);
            }
            else
            {
                q = q.Include(c => c.Children).WhereIf(input.Code.IsNotNullOrWhiteSpaceBXJG(), c => c.Code != input.Code && c.Code.StartsWith(input.Code));
            }

            q = q.OrderBy(c => c.Code);
            var list = await q.ToListAsync();
            var dtos = new List<OuDto>();
            foreach (var item in list)
            {
                var item2 = item as OrganizationUnitEntity;
                dtos.Add(new OuDto
                {
                    Checked = default,
                    Code = item.Code,
                    IconCls = "ou",
                    Id = item.Id,
                    Text = item.DisplayName,
                    //State = item.Children.Count>0"opend":"closed"
                    ParentId = item.ParentId,
                    OUType = item2 == default ? OUType.HeadOffice : item2.OUType,
                    DisplayName = item.DisplayName,
                });
            }
            if (!input.IsOnlyLoadChild)
            {
                dtos.ForEach(c =>
                {
                    c.Children = dtos.Where(d => d.ParentId == c.Id).ToList();
                    c.ChildrenCount = c.Children.Count;
                });
                if (input.Code.IsNullOrEmpty())
                    return dtos.Where(c => !c.ParentId.HasValue).ToList();
                return dtos.Where(c => c.Parent.Code == input.Code).ToList();
            }
            else
            {
                return dtos;
            }
        }

        public Task<IList<GeneralTreeComboboxDto>> GetNodesForSelectAsync(GeneralTreeGetForSelectInput input)
        {
            throw new NotImplementedException();
        }
    }
}

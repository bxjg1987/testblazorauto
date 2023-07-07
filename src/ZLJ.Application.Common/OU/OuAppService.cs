using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Organizations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.BaseInfo;

namespace ZLJ.App.Common.OU
{
    /// <summary>
    /// 提供公司、部门下拉树形数据；登陆用户即可访问，将来增加权限依赖
    /// </summary>
    [AbpAuthorize]
    public class OuAppService : Abp.Application.Services.ApplicationService
    {
        IRepository<OrganizationUnit, long> repository;

        public OuAppService(IRepository<OrganizationUnit, long> repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// 提供公司、部门下拉树形数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public async Task<IList<OuDto>> GetListAsync(GetListInput input)
        {
            if (input.Code.IsNullOrWhiteSpace() && input.ParentId.HasValue && input.ParentId.Value > 0)
                input.Code = await repository.GetAll().Where(c => c.Id == input.ParentId).Select(c => c.Code).SingleAsync();

            var q = repository.GetAll();
            if (input.WhatType == 0)
                q = q.OfType<OrganizationUnitEntity>();
            else
                q = q.OfType<ZLJ.Customer.CustomerOUEntity>();
            q = q.AsNoTrackingWithIdentityResolution()
                 .Include(c => c.Children)
                 .WhereIf(!input.Code.IsNullOrWhiteSpace(), c => c.Code.StartsWith(input.Code))
                 .WhereIf(input.ForType <= 0 || !input.ParentText.IsNullOrWhiteSpace(), c => c.Code != input.Code);

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
                    Id = item.Id.ToString(),
                    Text = item.DisplayName,
                    //State = item.Children.Count>0"opend":"closed"
                    ParentId = item.ParentId?.ToString(),
                    OUType = item2.OUType
                });
            }
            dtos.ForEach(c => c.Children = dtos.Where(d => d.ParentId == c.Id).ToList());

            var p = list.SingleOrDefault(c => c.Code == input.Code);
            if (p != null)
                dtos = dtos.Where(c => c.Code == p.Code).ToList();
            else
                dtos = dtos.Where(c => c.ParentId.IsNullOrWhiteSpace()).ToList();

            if (input.ForType <= 0)
                return dtos;

            var pDto = new OuDto { IconCls = "ou" };
            if (input.ForType == 1)
            {
                if (input.ParentText.IsNullOrWhiteSpace())
                {
                    if (p != null)
                        pDto.Text = "==" + p.DisplayName + "==";
                    else
                        pDto.Text = "==公司和部门==";
                }
                else
                    pDto.Text = input.ParentText;
            }

            if (input.ForType >= 2)
            {
                if (input.ParentText.IsNullOrWhiteSpace())
                    pDto.Text = "==请选择==";
                else
                    pDto.Text = input.ParentText;

            }

            dtos.Insert(0, pDto);
            return dtos;
        }
    }
}

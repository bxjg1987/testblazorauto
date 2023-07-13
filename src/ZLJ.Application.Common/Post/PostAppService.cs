using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.BaseInfo.Post;
using Microsoft.EntityFrameworkCore;
using Abp.Organizations;
using Abp.Extensions;
using Abp.Linq.Extensions;
using System.Linq.Expressions;
using Abp.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;

namespace ZLJ.App.Common.Post
{
    [Abp.Authorization.AbpAuthorize]
    [Abp.Domain.Uow.UnitOfWork(false)]
    public class PostAppService : CommonBaseApplicationService
    {
        private readonly Lazy<IQueryable<PostEntity>> repository;
        private readonly Lazy<IQueryable<OrganizationUnitRole>> ouRoleRepository;
        private readonly Lazy<IQueryable<OrganizationUnit>> ouRepository;

        public PostAppService(IRepository<PostEntity, int> repository, IRepository<OrganizationUnitRole, long> ouRoleRepository, IRepository<OrganizationUnit, long> ouRepository)
        {
            this.repository = new Lazy<IQueryable<PostEntity>>(() => repository.GetAll().AsNoTrackingWithIdentityResolution());
            this.ouRoleRepository = new Lazy<IQueryable<OrganizationUnitRole>>(() => ouRoleRepository.GetAll().AsNoTrackingWithIdentityResolution());
            this.ouRepository = new Lazy<IQueryable<OrganizationUnit>>(() => ouRepository.GetAll().AsNoTrackingWithIdentityResolution());
        }
        private class TempQueryModel
        {
            public PostEntity post { get; set; }
            public OrganizationUnit ou { get; set; }
        }
        //[HttpGet]
        public async Task<List<PostDto>> GetListAsync(GetListInput input)
        {
            if (input.OuCodes == default)
                input.OuCodes = new List<string>(0);

            if (input.OuIds == default)
                input.OuIds = new long[0];

            if (input.OuCodes.Count == 0 && input.OuIds.Length > 0)
            {
                input.OuCodes = await ouRepository.Value.Where(c => input.OuIds.Contains(c.Id)).Select(c => c.Code).ToListAsync();
            }

            var q = from post in repository.Value
                    join ouRole in ouRoleRepository.Value on post.Id equals ouRole.RoleId into ouRoles
                    from ouRole in ouRoles.DefaultIfEmpty()
                    join ou in ouRepository.Value on ouRole.OrganizationUnitId equals ou.Id into ous
                    from ou in ous.DefaultIfEmpty()
                    select new TempQueryModel { post = post, ou = ou };
            //经过测试 用元组 下面的条件，ef无法翻译
            Expression<Func<TempQueryModel, bool>> tj = c => input.OuCodes.Count ==0;
            foreach (var item in input.OuCodes)
            {
                tj = tj.Or(c => c.ou.Code.StartsWith(item));
            }
            q = q.Where(tj);

            var q2 = from post in q.Select(c => c.post).Distinct()
                     join ouRole in ouRoleRepository.Value on post.Id equals ouRole.RoleId into ouRoles
                     from ouRole in ouRoles.DefaultIfEmpty()
                     join ou in ouRepository.Value on ouRole.OrganizationUnitId equals ou.Id into ous
                     from ou in ous.DefaultIfEmpty()
                     select new { post, ou };

            var list = await q2.ToListAsync();
            var group = list.GroupBy(c => c.post, c => c.ou);
            var dtos = new List<PostDto>();
            foreach (var post in group)
            {
                var dto = ObjectMapper.Map<PostDto>(post.Key);
                dto.OuText = string.Join(',', post.Where(c => c != default).Select(c => c.DisplayName));
                dtos.Add(dto);
            }
            return dtos;
        }
    }
}
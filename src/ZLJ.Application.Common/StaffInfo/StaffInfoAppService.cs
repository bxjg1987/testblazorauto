using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.BaseInfo.StaffInfo;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ZLJ.Core.BaseInfo;
using Abp.Organizations;
using Abp.Authorization.Users;
using ZLJ.Core.BaseInfo.Post;
using BXJG.Utils.Localization;
using Microsoft.AspNetCore.Mvc;
using Abp.Threading;

namespace ZLJ.Application.Common.StaffInfo
{


    /// <summary>
    /// 前后台公用的，主要用来查询员工信息，登陆用户就可以访问，不验证功能权限
    /// </summary>
    [UnitOfWork(false)]
    [AbpAuthorize]
    public class StaffInfoAppService : CommonBaseAppService
    {
        private readonly Lazy<IQueryable<OrganizationUnitEntity>> ouQuery;
        private readonly Lazy<IQueryable<OrganizationUnitRole>> ouRoleQuery;
        private readonly Lazy<IQueryable<UserOrganizationUnit>> ouUserQuery;
        private readonly Lazy<IQueryable<UserRole>> userRoleQuery;
        private readonly Lazy<IQueryable<StaffInfoEntity>> userQuery;
        private readonly Lazy<IQueryable<PostEntity>> postQuery;

        public StaffInfoAppService(IRepository<StaffInfoEntity, long> repository,
                                   IRepository<OrganizationUnitEntity, long> ouRepository,
                                   IRepository<OrganizationUnitRole, long> ouRoleRepository,
                                   IRepository<UserOrganizationUnit, long> ouUserRepository,
                                   IRepository<UserRole, long> userRoleRepository,
                                   IRepository<PostEntity> postRepository)
        {
            userQuery = new Lazy<IQueryable<StaffInfoEntity>>(() => repository.GetAll().AsNoTrackingWithIdentityResolution());
            userRoleQuery = new Lazy<IQueryable<UserRole>>(() => userRoleRepository.GetAll().AsNoTrackingWithIdentityResolution());
            ouUserQuery = new Lazy<IQueryable<UserOrganizationUnit>>(() => ouUserRepository.GetAll().AsNoTrackingWithIdentityResolution());
            ouRoleQuery = new Lazy<IQueryable<OrganizationUnitRole>>(() => ouRoleRepository.GetAll().AsNoTrackingWithIdentityResolution());
            ouQuery = new Lazy<IQueryable<OrganizationUnitEntity>>(() => ouRepository.GetAll().AsNoTrackingWithIdentityResolution());
            postQuery = new Lazy<IQueryable<PostEntity>>(() => postRepository.GetAll().AsNoTrackingWithIdentityResolution());
        }

        private IQueryable<QueryTemp> ApplyCondition(IQueryable<QueryTemp> q, GetTotalInput input)
        {
            return q.WhereIf(!input.Keyword.IsNullOrWhiteSpace(), c => c.Staff.Name.Contains(input.Keyword) ||
                                                                       c.Staff.Pinyin.Contains(input.Keyword) ||
                                                                       c.Staff.PhoneNumber.Contains(input.Keyword) ||
                                                                       c.Staff.CurrentAddress.Contains(input.Keyword))
                    .WhereIf(!input.OuCode.IsNullOrWhiteSpace(), c => c.Ou.Code.StartsWith(input.OuCode))
                    .Where(x=>x.Staff.IsActive)
                    .WhereIf(input.PostId.HasValue, c => c.Post.Id == input.PostId.Value)
                    .WhereIf(!input.AreaCode.IsNullOrWhiteSpace(), c => c.Staff.Area.Code.StartsWith(input.OuCode))
                    .ApplyDynamicCondtion<QueryTemp>(input.Conditions??new List<ConditionFieldDefine>());
        }

        //前期用join查询的方式，后期考虑冗余字段方式提高查询性能（注意依赖数据更新的情况）

        /// <summary>
        /// 获取员工分页列表以供选择
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedResultDto<Dto>> GetListAsync(GetListInput input)
        {
            var r = new PagedResultDto<Dto>();
            var q = GetFullQuery();
            q = ApplyCondition(q, input);

            var ss = q.ToQueryString();

            q = q.OrderBy("Staff." + input.Sorting);

            var q2 = q.GroupBy(c => c.Staff.Id).Select(c => c.Key);
            
            q = q.OrderBy("Staff." + input.Sorting);

            r.TotalCount = await q2.CountAsync(CancellationTokenProvider.Token);

            q2 = q2.PageBy(input);
            var list = await q2.ToListAsync(CancellationTokenProvider.Token);
            var roleAndOus = await GetRoleAndOusAsync(list);
            var dtos = new List<Dto>();
            foreach (var item in roleAndOus)
            {
                var dto = ObjectMapper.Map<Dto>(item.Key);
                dto.PostsText = string.Join(',', item.DistinctBy(c=>c.Post?.Id).Select(c => c.Post?.DisplayName));
                dto.GenderText = item.Key.Gender.ToLocalizationString();
                dto.OusText = string.Join(',', item.DistinctBy(c => c.Ou?.Id).Select(c => c.Ou?.DisplayName));
                dtos.Add(dto);
            }
            r.Items = dtos;
            return r;
        }

        public async Task<int> GetTotalAsync(GetTotalInput input)
        {
            var q = GetFullQuery();
            q = ApplyCondition(q, input);
            return await q.GroupBy(c => c.Staff.Id).CountAsync(CancellationTokenProvider.Token);
        }

        private IQueryable<QueryTemp> GetFullQuery()
        {
            var q = from staff in userQuery.Value
                    join ouUser in ouUserQuery.Value on staff.Id equals ouUser.UserId into ouUsers
                    from ouUser in ouUsers.DefaultIfEmpty()
                    join userRole in userRoleQuery.Value on staff.Id equals userRole.UserId into userRoles
                    from userRole in userRoles.DefaultIfEmpty()
                    join ouRole in ouRoleQuery.Value on userRole.RoleId equals ouRole.RoleId into ouRoles
                    from ouRole in ouRoles.DefaultIfEmpty()
                    join post in postQuery.Value.OfType<PostEntity>() on userRole.RoleId equals post.Id into posts
                    from post in posts.DefaultIfEmpty()
                    from ou in ouQuery.Value.Where(c => c.Id == ouUser.OrganizationUnitId ||
                                                        c.Id == ouRole.OrganizationUnitId)
                                            //.DistinctBy(c=>c.Id)
                                            .DefaultIfEmpty()//加上就是outer apply  不加就是cross apply
                    select new QueryTemp
                    {
                        Staff = staff,
                        Post = post,
                        Ou = ou
                    };

            return q;
        }

        private async Task<List<IGrouping<StaffInfoEntity, QueryTemp>>> GetRoleAndOusAsync(IList<long> ids)
        {
            //var postQuery = from userRole in userRoleRepository.GetAll()
            //                                                   .AsNoTrackingWithIdentityResolution()
            //                                                   .Where(c => ids.Contains(c.UserId))
            //                join post in postRepository.GetAll().AsNoTrackingWithIdentityResolution() on userRole.RoleId equals post.Id into roles
            //                from post in roles.DefaultIfEmpty()
            //                select post;
            //var posts = await postQuery.ToListAsync();
            //var postIds = posts.Select(c => c.Id);
            //var ouRoleIds = organizationUnitRoleRepository.GetAll()
            //                                                           .AsNoTrackingWithIdentityResolution()
            //                                                           .Where(c => postIds.Contains(c.RoleId))
            //                                                           .Select(c => c.OrganizationUnitId).ToListAsync();
            //ef目前好像还不支持Union，所以分次查询会出现4次查询

            var q = from user in userQuery.Value.Where(c => ids.Contains(c.Id))

                    join userRole in userRoleQuery.Value on user.Id equals userRole.UserId into userRoles
                    from userRole in userRoles.DefaultIfEmpty()

                    join ouRole in ouRoleQuery.Value on userRole.RoleId equals ouRole.RoleId into ouRoles
                    from ouRole in ouRoles.DefaultIfEmpty()

                    join ouUser in ouUserQuery.Value on user.Id equals ouUser.UserId into ouUsers
                    from ouUser in ouUsers.DefaultIfEmpty()

                    join post in postQuery.Value on userRole.RoleId equals post.Id into roles
                    from post in roles.DefaultIfEmpty()

                    from ou in ouQuery.Value.Where(c => c.Id == ouRole.OrganizationUnitId ||
                                                        c.Id == ouUser.OrganizationUnitId)
                                            //.DistinctBy(c => c.Id)
                                            .DefaultIfEmpty()
                    select new QueryTemp
                    {
                        Ou = ou,
                        Post = post,
                        Staff = user
                    };

            //var sql = q.ToQueryString();
            var list = await q.ToListAsync(CancellationTokenProvider.Token);
            var sdf = list.GroupBy(c => c.Staff);
            var df = new List<IGrouping<StaffInfoEntity, QueryTemp>>();
            foreach (var item in ids)
            {
                df.Add(sdf.Single(c => c.Key.Id == item));
            }
            return df;
        }
    }
}
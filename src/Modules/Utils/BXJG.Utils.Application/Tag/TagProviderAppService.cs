using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using BXJG.Utils.Application.Share.Tag;
using BXJG.Utils.Share.Tag;
using BXJG.Utils.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Tag
{


    /*
     * 这个应该用controller，性能更好
     */

    public class TagProviderAppService : BXJGUtilsBaseAppService
    {
        public BXJGUtilsModuleConfig BXJGUtilsModuleConfig { get; set; }
        //public IRepository<TagEntity, Guid> Repository { get; set; }
        public IPermissionDependencyContext PermissionDependencyContext { get; set; }
        public IScopedIocResolver ScopedIocResolver { get; set; }
        /// <summary>
        /// 获取可选的tag
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="AbpAuthorizationException"></exception>
        [Obsolete("这里要去掉，用contoller，不要输入参数模型，直接httpget，尤其是考虑查询实体id是guid，无需使用实体类型参数的情况")]
        public async Task<List<SelectableTagDto>> GetSelectableList(GetSelectableInput input)
        {
            //var q = await Repository.GetAllReadonlyAsync();
            var key = input.EntityType + (input.PropertyName.IsNotNullOrWhiteSpaceBXJG() ? "." + input.PropertyName : "");


            var list = new List<SelectableTagDto>();
            if (!BXJGUtilsModuleConfig.SelectableTagProviders.TryGetValue(key, out var providers))
                return list;

            if (!await providers.permissionDependency.IsSatisfiedAsync(PermissionDependencyContext))
                throw new AbpAuthorizationException();

            var ctx = new SelectableTagContext(ScopedIocResolver, input.EntityType, input.PropertyName, input.Top);
            //由于不晓得后续提供器获取的数据，同一个tag的排序是否较高，所以每个提供器都获取top条数据
            foreach (var item in providers.providers)
            {
                list.AddRange(await item.Invoke(ctx));
            }
            //若前端提交时指定了，或定义数据源时指定了，需要冲tag表后去数据源
            if (input.LoadFromDb == true || providers.loadFromDb)
            {
                list.AddRange(await TagManager.GetSelectableList(ctx));
            }

            return list.OrderByDescending(x => x.OrderIndex)
                       .DistinctBy(x => x.TagName)
                       .Take(input.Top)
                       .ToList();

        }
    }
}
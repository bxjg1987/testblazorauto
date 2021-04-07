using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Dependency;

namespace BXJG.DynamicAssociateEntity
{
    public class DynamicAssociateEntityAppService : ApplicationService
    {
        protected readonly DynamicAssociateEntityDefineManager dynamicAssociateEntityDefineManager;
        protected readonly IIocResolver iocResolver;

        public DynamicAssociateEntityAppService(IIocResolver iocResolver, DynamicAssociateEntityDefineManager dynamicAssociateEntityDefineManager)
        {
            this.iocResolver = iocResolver;
            this.dynamicAssociateEntityDefineManager = dynamicAssociateEntityDefineManager;
        }
        /// <summary>
        /// 获取动态关联的实体的列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<object>> GetAllAsync(GetAllInput input)
        {
            var define = dynamicAssociateEntityDefineManager.Defines[input.DefineName];
            var service = this.iocResolver.Resolve(define.ServiceType) as IDynamicAssociateEntityService;
            var list = await service.GetAllAsync(input.ParentId, input.Keyword, input.Sorting, input.SkipCount, input.MaxResultCount);
            return list;
        }
        /// <summary>
        /// sss
        /// </summary>
        public void ddd() { 
        
        }
    }

    public class GetAllInput : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 如：product
        /// </summary>
        public string DefineName { get; set; }
        public string Keyword { get; set; }
        /// <summary>
        /// 有级联关联时，比如关联到订单明细，那么在查询订单明细时需要提供所属订单的id
        /// </summary>
        public string ParentId { get; set; }
    }
}

using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
namespace BXJG.DynamicAssociateEntity
{
    public class DynamicAssociateEntityAppService : ApplicationService
    {
        protected readonly DynamicAssociateEntityDefineManager dynamicAssociateEntityDefineManager;
        protected readonly Abp.Dependency.IIocResolver iocResolver;

        public DynamicAssociateEntityAppService(Abp.Dependency.IIocResolver iocResolver, DynamicAssociateEntityDefineManager dynamicAssociateEntityDefineManager)
        {
            this.iocResolver = iocResolver;
            this.dynamicAssociateEntityDefineManager = dynamicAssociateEntityDefineManager;
        }

        public async Task<PagedResultDto<object>> GetAllAsync(GetAllInput input)
        {
            var define = dynamicAssociateEntityDefineManager.Defines[input.DefineName];
            var service = this.iocResolver.Resolve(define.ServiceType2) as IDynamicAssociateEntityService2;
            return await service.GetAllAsync(input.ParentId, input.Keyword, input.Sorting, input.SkipCount, input.MaxResultCount);
        }
    }

    public class GetAllInput : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 如：product
        /// </summary>
        public string DefineName { get; set; }
        public string Keyword { get; set; }
        public string ParentId { get; set; }
    }
}

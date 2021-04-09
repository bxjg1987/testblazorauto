using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Localization;
using Abp.Extensions;
using Abp.Authorization;

namespace BXJG.DynamicAssociateEntity
{
    [AbpAuthorize]
    public class DynamicAssociateEntityAppService : ApplicationService
    {
        protected readonly DynamicAssociateEntityDefineManager dynamicAssociateEntityDefineManager;
        protected readonly IIocResolver iocResolver;
        protected readonly ILocalizationManager localizationManager;

        public DynamicAssociateEntityAppService(IIocResolver iocResolver, DynamicAssociateEntityDefineManager dynamicAssociateEntityDefineManager, ILocalizationManager localizationManager)
        {
            this.iocResolver = iocResolver;
            this.dynamicAssociateEntityDefineManager = dynamicAssociateEntityDefineManager;
            this.localizationManager = localizationManager;
        }
        /// <summary>
        /// 获取定义
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public IList<DynamicAssociateEntityDefineDto> GetDefines(string groupName)
        {
            //测试代码，可以删除
            //var obj = Class1.DtoMapToEntity(dynamicAssociateEntityDefineManager.GroupedDefines[groupName].LeafItems, new Dictionary<string, object> {
            //    { "article",3 } ,
            //    { "column",1 },
            //    { "equipment",6 }
            //});
            //var str = System.Text.Json.JsonSerializer.Serialize(obj);

            var es = dynamicAssociateEntityDefineManager.GroupedDefines[groupName].Items.Select(item => new DynamicAssociateEntityDefineDto
            {
                AssociateGranularity = item.AssociateGranularity,
                Required = item.Required,
                ChildName = item.Define.ChildName,
                DisplayFields = item.Define.DisplayFields.Select(qq => qq.Name).ToArray(),
                DisplayName = item.Define.DisplayName.Localize(this.localizationManager),
                Name = item.Define.Name,
                Control = item.Define.Control,
                NeedPagination = item.Define.NeedPagination,
                Fields = item.Define.Fields.Select(qq => new DynamicAssociateEntityDefineFieldDto
                {
                    DisplayFormatter = qq.DislayFormatter,
                    DisplayName = qq.DislayName.Localize(this.localizationManager),
                    DisplayWidth = qq.DislayWidth,
                    IsDisplayField = qq.IsDisplayField,
                    IsKey = qq.IsKey,
                    Name = qq.Name
                }).ToArray(),
                ParentName = item.Define.ParentName,
                KeyField = item.Define.KeyField.Name
            }).ToList();
            foreach (var item in es)
            {
                item.Child = es.SingleOrDefault(c => c.ParentName == item.Name);
            }
            return es.Where(c => c.ParentName.IsNullOrWhiteSpace()).ToList();
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
    /// <summary>
    /// 动态关联的目标数据定义
    /// </summary>
    public class DynamicAssociateEntityDefineDto
    {
        /// <summary>
        /// 全局唯一名称，如：equipment
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名，如：设备信息
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 字段定义列表
        /// </summary>
        public DynamicAssociateEntityDefineFieldDto[] Fields { get; set; }
        /// <summary>
        /// 级联关联时的父级名称，比如关联到订单明细时，则父级为order，子级为orderItem
        /// </summary>
        public string ParentName { get; set; }
        //public DynamicAssociateEntityDefineDto Parent { get; set; }//避免循环依赖
        /// <summary>
        /// 级联关联时的子级名称，比如关联到订单明细时，则父级为order，子级为orderItem
        /// </summary>
        public string ChildName { get; set; }
        /// <summary>
        /// 子级数据定义
        /// </summary>
        public DynamicAssociateEntityDefineDto Child { get; set; }
        /// <summary>
        /// 主键字段名
        /// </summary>
        public string KeyField { get; set; }
        /// <summary>
        /// 用作显示的列集合字段名
        /// </summary>
        public string[] DisplayFields { get; set; }
        /// <summary>
        /// 关联粒度，所有行都关联到同一个实体类型 还是每行关联到不同的实体类型
        /// </summary>
        public AssociateGranularity AssociateGranularity { get; set; }
        /// <summary>
        /// 控件
        /// </summary>
        public string Control { get; set; }
        /// <summary>
        /// 必填
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// 数据是否需要分页
        /// </summary>
        public bool NeedPagination { get; set; }
    }
    /// <summary>
    /// 动态关联数据字段定义
    /// </summary>
    public class DynamicAssociateEntityDefineFieldDto
    {
        /// <summary>
        /// 是否作为显示字段
        /// </summary>
        public bool IsDisplayField { get; set; }
        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsKey { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 字段显示名
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 显示格式
        /// </summary>
        public string DisplayFormatter { get; set; }
        /// <summary>
        /// 显示宽度，不同的前端需要的宽度不同，这里只是个默认值，或者作为百分比
        /// </summary>
        public int DisplayWidth { get; set; }

        //public int OrderIndex { get; set; }
    }
}

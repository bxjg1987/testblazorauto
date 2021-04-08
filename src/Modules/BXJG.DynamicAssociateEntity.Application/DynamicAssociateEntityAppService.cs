using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Localization;

namespace BXJG.DynamicAssociateEntity
{
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
            var r = new List<DynamicAssociateEntityDefineDto>();

            var es = dynamicAssociateEntityDefineManager.GroupedDefines[groupName].Items;
            foreach (var item in es)
            {
                var child = item.Define;
                while (child.Parent != null)
                {
                    child = child.Parent;
                }
                DynamicAssociateEntityDefineDto parent = null;
                while (child != null)
                {
                    var n = new DynamicAssociateEntityDefineDto
                    {
                        AssociateGranularity = item.AssociateGranularity,
                        ChildName = child.ChildName,
                        DisplayFields = child.DisplayFields.Select(qq => qq.Name).ToArray(),
                        DisplayName = child.DisplayName.Localize(this.localizationManager),
                        Name = child.Name,
                        Fields = child.Fields.Select(qq => new DynamicAssociateEntityDefineFieldDto
                        {
                            DislayFormatter = qq.DislayFormatter,
                            DislayName = qq.DislayName.Localize(this.localizationManager),
                            DislayWidth = qq.DislayWidth,
                            IsDisplayField = qq.IsDisplayField,
                            IsKey = qq.IsKey,
                            Name = qq.Name
                        }).ToArray(),
                        ParentName = child.ParentName,
                        KeyField = child.KeyField.Name
                    };
                    if (parent != null)
                        parent.Child = n;
                    else
                        r.Add(n);

                    parent = n;
                    child = child.Child;
                }
            }
            return r;
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
        public string DislayName { get; set; }
        /// <summary>
        /// 显示格式
        /// </summary>
        public string DislayFormatter { get; set; }
        /// <summary>
        /// 显示宽度，不同的前端需要的宽度不同，这里只是个默认值，或者作为百分比
        /// </summary>
        public int DislayWidth { get; set; }

        //public int OrderIndex { get; set; }
    }
}

using AutoMapper;
using BXJG.Utils.GeneralTree;
using BXJG.WorkOrder.WorkOrder;
using BXJG.WorkOrder.WorkOrderCategory;
using System.Text.Json;
namespace BXJG.WorkOrder
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region 基础工单
            CreateMap<OrderEntity, WorkOrderDto>();
            #endregion

            #region 工单分类
            //CreateMap(typeof(WorkOrderCategoryEditInput), typeof(CategoryEntity));
            //CreateMap(typeof(CategoryEntity), typeof(WorkOrderCategroyDto));
            CreateMap<WorkOrderCategoryEditInput, CategoryEntity>();

            CreateMap<WorkOrderTypeDto, WorkOrderCategoryTypeEntity>();
            //CreateMap<WorkOrderCategoryTypeEntity, WorkOrderTypeDto>();


            CreateMap<CategoryEntity, WorkOrderCategroyDto>();
            CreateMap<WorkOrderCategoryTypeEntity, CategoryWorkOrderTypeDto>();
            #endregion
        }
    }
}

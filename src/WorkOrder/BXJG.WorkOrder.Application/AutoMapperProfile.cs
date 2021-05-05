using AutoMapper;
using BXJG.GeneralTree;
using BXJG.WorkOrder.WorkOrder;
using BXJG.WorkOrder.WorkOrderCategory;
using System.Text.Json;
namespace BXJG.WorkOrder
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region 工单分类
            CreateMap(typeof(WorkOrderCategoryEditInput), typeof(CategoryEntity));
            CreateMap(typeof(CategoryEntity), typeof(WorkOrderCategroyDto));
            #endregion
        }
    }
}

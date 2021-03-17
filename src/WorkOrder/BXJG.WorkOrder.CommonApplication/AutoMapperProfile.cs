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
            //CreateMap(typeof(WorkOrderUpdateInput), typeof(OrderEntity)).DtoToEntity().ForMember("ContentType", opt => opt.Ignore());
            //CreateMap(typeof(ColumnEntity), typeof(ColumnDto)).EntityToDto();//可能是因为泛型原因，必须调用EntityToDto
            //CreateMap(typeof(ColumnEntity), typeof(ColumnTreeNodeDto)).EntityToComboTree();
            //CreateMap(typeof(ColumnEntity), typeof(ColumnCombboxDto)).EntityToCombobox();

            //CreateMap(typeof(WorkOrderCategoryEditInput), typeof(CategoryEntity));
            //CreateMap(typeof(CategoryEntity), typeof(WorkOrderCategroyDto));
            CreateMap(typeof(CategoryEntity), typeof(WorkOrderCategoryTreeNodeDto)).EntityToComboTree();
            CreateMap(typeof(CategoryEntity), typeof(WorkOrderCategoryComboboxItemDto)).EntityToCombobox();
            #endregion
        }
    }
}

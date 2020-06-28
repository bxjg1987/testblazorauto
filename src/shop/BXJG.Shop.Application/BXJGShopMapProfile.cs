using AutoMapper;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop
{
    public class BXJGShopMapProfile : Profile
    {
        public BXJGShopMapProfile()
        {
            #region 商品分类
            CreateMap<ItemCategoryEditDto, ItemCategoryEntity>();
            CreateMap<ItemCategoryEntity, ItemCategoryDto>();
            CreateMap<ItemCategoryEntity, ItemCategoryTreeNodeDto>().EntityToComboTree();
            CreateMap<ItemCategoryEntity, ItemCategoryCombboxDto>().EntityToCombobox();
            // CreateMap(typeof(ItemCategoryEditDto), typeof(ItemCategoryEntity<>)).DtoToEntity().ForMember("ContentType", opt => opt.Ignore());
            //CreateMap(typeof(ColumnEntity<>), typeof(ColumnDto)).EntityToDto();//可能是因为泛型原因，必须调用EntityToDto
            //CreateMap(typeof(ColumnEntity<>), typeof(ColumnTreeNodeDto)).EntityToComboTree();
            //CreateMap(typeof(ColumnEntity<>), typeof(ColumnCombboxDto)).EntityToCombobox();
            #endregion

        }
    }
}

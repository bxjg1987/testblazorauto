using AutoMapper;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue;
using BXJG.Shop.Common;
using BXJG.Shop.Common.Dto;
using BXJG.Shop.Customer;
using BXJG.Shop.Sale;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop
{
    public class BXJGShopMapProfile<TUser> : Profile
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

            #region 商城字典
            CreateMap<BXJGShopDictionaryEntity, DictionaryDto>();
            CreateMap<BXJGShopDictionaryEntity, DictionaryTreeNodeDto>().EntityToComboTree();
            CreateMap<BXJGShopDictionaryEntity, DictionaryCombboxDto>().EntityToCombobox();
            #endregion

            #region 上架信息/商品信息
            CreateMap<ItemEntity, ItemDto>()
               .ForMember(c => c.Images, opt => opt.MapFrom(d => d.Images.Split(',', System.StringSplitOptions.None)));

            CreateMap<ItemCreateDto, ItemEntity>()
               .ForMember(c => c.Images, opt => opt.MapFrom(c => string.Join(',', c.Images)));

            CreateMap<ItemUpdateDto, ItemEntity>()
               .ForMember(c => c.Images, opt => opt.MapFrom(c => string.Join(',', c.Images)));
            #endregion

            #region 显示给顾客的商品信息
            CreateMap<ItemEntity, FrontItemDto>()
               .ForMember(c => c.Images, opt => opt.MapFrom(d => d.Images.Split(',', System.StringSplitOptions.None)));
            #endregion

            #region 前端顾客和订单相关东东
            CreateMap<OrderItemEntity<TUser>, CustomerOrderItemDto>();
            CreateMap<OrderEntity<TUser>, CustomerOrderDto>();
            #endregion

            #region 会员
            //https://automapper.readthedocs.io/en/latest/Open-Generics.html
            //文档说了不能使用泛型方法来创建开放泛型映射，但即使是一个typeof()方式 也不行 所以只能手动来
            //CreateMap<CustomerUpdateDto, CustomerEntity<>>()
            //    .ForMember(c => c.User, opt => opt.Ignore());
            #endregion

            #region 后台管理员+订单
            CreateMap<OrderEntity<TUser>, OrderDto>();
            #endregion

            #region 后台管理对顾客信息的管理时使用的dto映射
            CreateMap<CustomerEntity<TUser>, CustomerDto>();// (typeof(CustomerEntity<,>), typeof(CustomerDto));
            #endregion

            //.ForMember(c => c.IsTreeText, opt => opt.MapFrom(c => c.IsTree.ToString().UtilsL()))
            //.ForMember(c => c.IsSysDefineText, opt => opt.MapFrom(c => c.IsSysDefine.ToString().UtilsL()))

            //CreateMap<GeneralTreeEditDto, GeneralTreeEntity>()
            //  .

            //// Role and permission
            //CreateMap<Permission, string>().ConvertUsing(r => r.Name);
            //CreateMap<RolePermissionSetting, string>().ConvertUsing(r => r.Name);

            //CreateMap<CreateRoleDto, Role>();

            //CreateMap<RoleDto, Role>();

            //CreateMap<Role, RoleDto>().ForMember(x => x.GrantedPermissions,
            //    opt => opt.MapFrom(x => x.Permissions.Where(p => p.IsGranted)));

            //CreateMap<Role, RoleListDto>();
            //CreateMap<Role, RoleEditDto>();
            //CreateMap<Permission, FlatPermissionDto>();
        }
    }
}

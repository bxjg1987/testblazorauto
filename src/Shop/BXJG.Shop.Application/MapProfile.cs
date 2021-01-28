using Abp.Application.Services.Dto;
using AutoMapper;
using AutoMapper.Configuration;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue;
using BXJG.Shop.Customer;
using BXJG.Shop.Sale;
using BXJG.Shop.ShoppingCart;
using BXJG.Shop.ShoppingCart.Customer;
using BXJG.Utils.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace BXJG.Shop
{
    public class MapProfile : Profile
    {
        public MapProfile(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            var serverUrl = configuration["App:ServerRootAddress"];

            #region 商品分类
            CreateMap<ProductCategoryEditDto, ProductCategoryEntity>();
            CreateMap<ProductCategoryEntity, ProductCategoryDto>()
                .ForMember(c => c.Icon, opt => opt.MapFrom(d => d.Icon.Replace(@"\", @"/").Replace(@"//", @"/").Replace(@"//", @"/")))
                .ForMember(c => c.Image1, opt => opt.MapFrom(d => d.Image1.Replace(@"\", @"/").Replace(@"//", @"/").Replace(@"//", @"/")))
                .ForMember(c => c.Image2, opt => opt.MapFrom(d => d.Image2.Replace(@"\", @"/").Replace(@"//", @"/").Replace(@"//", @"/")));
            CreateMap<ProductCategoryEntity, ProductCategoryTreeNodeDto>().EntityToComboTree()
                .ForMember(c => c.Icon, opt => opt.MapFrom(d => d.Icon.Replace(@"\", @"/").Replace(@"//", @"/").Replace(@"//", @"/")))
                .ForMember(c => c.Image1, opt => opt.MapFrom(d => d.Image1.Replace(@"\", @"/").Replace(@"//", @"/").Replace(@"//", @"/")))
                .ForMember(c => c.Image2, opt => opt.MapFrom(d => d.Image2.Replace(@"\", @"/").Replace(@"//", @"/").Replace(@"//", @"/")));
            CreateMap<ProductCategoryEntity, ProductCategoryCombboxDto>().EntityToCombobox()
                .ForMember(c => c.Icon, opt => opt.MapFrom(d => d.Icon.Replace(@"\", @"/").Replace(@"//", @"/").Replace(@"//", @"/")))
                .ForMember(c => c.Image1, opt => opt.MapFrom(d => d.Image1.Replace(@"\", @"/").Replace(@"//", @"/").Replace(@"//", @"/")))
                .ForMember(c => c.Image2, opt => opt.MapFrom(d => d.Image2.Replace(@"\", @"/").Replace(@"//", @"/").Replace(@"//", @"/")));
            //CreateMap(typeof(ItemCategoryEditDto), typeof(ItemCategoryEntity<>)).DtoToEntity().ForMember("ContentType", opt => opt.Ignore());
            //CreateMap(typeof(ColumnEntity<>), typeof(ColumnDto)).EntityToDto();//可能是因为泛型原因，必须调用EntityToDto
            //CreateMap(typeof(ColumnEntity<>), typeof(ColumnTreeNodeDto)).EntityToComboTree();
            //CreateMap(typeof(ColumnEntity<>), typeof(ColumnCombboxDto)).EntityToCombobox();
            #endregion

            #region 上架信息/商品信息
            CreateMap<ProductEntity, ProductDto>()
                .ForMember(c => c.Images, opt => opt.MapFrom(d => d.GetImages().Select(e => new FileDto { FilePath = e.Value, ThumPath = e.Key })));

            //CreateMap<ProductCreateDto, ProductEntity>()
            //   .ForMember(c => c.Images, opt => opt.MapFrom(c => string.Join(',', c.Images)));

            CreateMap<ProductUpdateDto, ProductEntity>()
               .ForMember(c => c.Images, opt => opt.MapFrom(c => string.Join(',', c.Images)));
            //sku
            //CreateMap<SkuEntity, SkuEditDto>().ForMember(c=>c.DynamicEntityPropertyValues,c=>c.Ignore());
            CreateMap<SkuEntity, SkuDto>();
            //.ForMember(c => c.DynamicEntityProperty1Text,
            //           opt => opt.MapFrom(c =>
            //               c.DynamicEntityProperty1.DynamicProperty.DynamicPropertyValues.Count > 0 ? c.DynamicEntityProperty1.DynamicProperty.DynamicPropertyValues.Single(d => d.Id.ToString() == c.DynamicEntityProperty1Value).Value : ""
            //            ))
            //.ForMember(c => c.DynamicEntityProperty2Text,
            //           opt => opt.MapFrom(c =>
            //               c.DynamicEntityProperty2.DynamicProperty.DynamicPropertyValues.Count > 0 ? c.DynamicEntityProperty2.DynamicProperty.DynamicPropertyValues.Single(d => d.Id.ToString() == c.DynamicEntityProperty2Value).Value : ""
            //            ))
            //.ForMember(c => c.DynamicEntityProperty3Text,
            //           opt => opt.MapFrom(c =>
            //               c.DynamicEntityProperty3.DynamicProperty.DynamicPropertyValues.Count > 0 ? c.DynamicEntityProperty3.DynamicProperty.DynamicPropertyValues.Single(d => d.Id.ToString() == c.DynamicEntityProperty3Value).Value : ""
            //            ))
            //.ForMember(c => c.DynamicEntityProperty4Text,
            //           opt => opt.MapFrom(c =>
            //               c.DynamicEntityProperty4.DynamicProperty.DynamicPropertyValues.Count > 0 ? c.DynamicEntityProperty4.DynamicProperty.DynamicPropertyValues.Single(d => d.Id.ToString() == c.DynamicEntityProperty4Value).Value : ""
            //            ))
            //.ForMember(c => c.DynamicEntityProperty5Text,
            //           opt => opt.MapFrom(c =>
            //               c.DynamicEntityProperty5.DynamicProperty.DynamicPropertyValues.Count > 0 ? c.DynamicEntityProperty5.DynamicProperty.DynamicPropertyValues.Single(d => d.Id.ToString() == c.DynamicEntityProperty5Value).Value : ""
            //            ));
            CreateMap<SkuEditDto, SkuEntity>();
            #endregion

            #region 显示给顾客的商品信息
            CreateMap<ProductEntity, FrontProductDto>()
                .ForMember(c => c.Images, opt => opt.MapFrom(d => d.GetImages().Select(e => new FileDto { FilePath = serverUrl + e.Value.Replace(@"\", @"/").Replace(@"//", @"/").Replace(@"//", @"/"), ThumPath = serverUrl + e.Key.Replace(@"\", @"/").Replace(@"//", @"/").Replace(@"//", @"/") })));
            #endregion

            #region 前端顾客和订单相关东东
            //CreateMap<OrderItemEntity, CustomerOrderItemDto>();
            CreateMap<OrderEntity, CustomerOrderDto>();
            #endregion

            #region 会员
            //https://automapper.readthedocs.io/en/latest/Open-Generics.html
            //文档说了不能使用泛型方法来创建开放泛型映射，但即使是一个typeof()方式 也不行 所以只能手动来
            //CreateMap<CustomerUpdateDto, CustomerEntity<>>()
            //    .ForMember(c => c.User, opt => opt.Ignore());
            #endregion

            #region 后台管理员+订单
            CreateMap<OrderEntity, OrderDto>();
            #endregion

            #region 后台管理对顾客信息的管理时使用的dto映射
            CreateMap<CustomerEntity, CustomerDto>();// (typeof(CustomerEntity<,>), typeof(CustomerDto));
            #endregion

            #region 购物车
            CreateMap<ShoppingCartItemEntity, GetItemOutput>();
            CreateMap<ShoppingCartEntity, GetOutput>();
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

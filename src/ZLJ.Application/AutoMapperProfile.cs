using System.Linq;
using AutoMapper;
using Abp.Authorization;
using Newtonsoft.Json;
using BXJG.Utils.Localization;
using BXJG.Shop.Common.Dto;
using BXJG.Shop.Catalogue;
using BXJG.Shop.Customer;
using ZLJ.Administrative;

namespace ZLJ
{
    /*
     * 商城模块中的所有dto映射定义在这里
     * 某些简单的情况直接使用Attribute形式定义映射
     * 不要过度依赖映射，因为在应用逻辑中对实体赋值 也是业务逻辑的一部分，在具体的方法中进行赋值更能体现业务，映射只是赋值作用
     * 通常非关键性字段、且很多地方的赋值规则一样时才使用自动映射
     * 某些时候可能需要将dto传入领域层，请不要直接传递dto，领域服务应该定义单独的模型来接受这些参数，或者干脆定义具体的参数列表。最好是使用前者，因为这样可以使用自动映射
     */
    /// <summary>
    /// 
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public AutoMapperProfile()
        {
            CreateMap<AdministrativeEntity, AdministrativeDto>();

            //CreateMap<BXJGShopDictionaryEntity, DictionaryDto>()
            //    .ForMember(c => c.ExtData, opt => opt.MapFrom(c => JsonConvert.DeserializeObject<dynamic>(c.ExtensionData)))
            //    .ForMember(c => c.Children, opt => opt.Ignore());

            //#region 上架信息/商品信息
            //CreateMap<ItemEntity, ItemDto>()
            //   .ForMember(c => c.Images, opt => opt.MapFrom(d => d.Images.Split(',', System.StringSplitOptions.None)));

            //CreateMap<ItemCreateDto, ItemEntity>()
            //   .ForMember(c => c.Images, opt => opt.MapFrom(c => string.Join(',', c.Images)));

            //CreateMap<ItemUpdateDto, ItemEntity>()
            //   .ForMember(c => c.Images, opt => opt.MapFrom(c => string.Join(',', c.Images)));
            //#endregion

            //#region 显示给顾客的商品信息
            //CreateMap<ItemEntity, FrontItemDto>()
            //   .ForMember(c => c.Images, opt => opt.MapFrom(d => d.Images.Split(',', System.StringSplitOptions.None)));
            //#endregion


            //#region 会员
            ////https://automapper.readthedocs.io/en/latest/Open-Generics.html
            ////文档说了不能使用泛型方法来创建开放泛型映射，但即使是一个typeof()方式 也不行 所以只能手动来
            ////CreateMap<CustomerUpdateDto, CustomerEntity<>>()
            ////    .ForMember(c => c.User, opt => opt.Ignore());
            //#endregion

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

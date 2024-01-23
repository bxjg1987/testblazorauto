using System.Linq;
using AutoMapper;
using Abp.Authorization;
using Newtonsoft.Json;
using BXJG.Utils.Localization;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.GeneralTree;

namespace BXJG.Utils.Application.GeneralTree
{
    public class GeneralTreeMapProfile : Profile
    {
        public GeneralTreeMapProfile()
        {

            //CreateMap(typeof(GeneralTreeEntity<>), typeof(GeneralTreeGetTreeNodeBaseDto<>))
            //    .IncludeAllDerived()
            //    .ForMember("State", opt => opt.Ignore())
            //    .ForMember("ExtData", opt => opt.Ignore());

            CreateMap<DataDictionaryEntity, DataDictionaryDto>();//.IncludeBase(typeof(GeneralTreeEntity<>), typeof(GeneralTreeGetTreeNodeBaseDto<>));
            CreateMap<DataDictionaryDto, DataDictionaryEditDto>();
            CreateMap<DataDictionaryEditDto, DataDictionaryEntity>();
            //经过测试,在有泛型的场景中AutoMapper使用继承并不能达到预期效果。因此使用扩展方法形式配置父类映射
            //CreateMap(typeof(GeneralTreeEntity<>), typeof(GeneralTreeNodeDto<>))
            //    .IncludeAllDerived()
            //    .ForMember("Text", opt => opt.MapFrom("DisplayName"))
            //    .ForMember("IconCls", opt => opt.Ignore())
            //    .ForMember("Checked", opt => opt.Ignore())
            //    .ForMember("State", opt => opt.Ignore())
            //    .ForMember("ExtData", opt => opt.Ignore());

            CreateMap<DataDictionaryEntity, GeneralTreeNodeDto>().EntityToComboTree();//.IncludeBase(typeof(GeneralTreeEntity<>), typeof(GeneralTreeNodeDto<>));

            CreateMap<DataDictionaryEntity, GeneralTreeComboboxDto>().EntityToCombobox();

            //.ForMember(c => c.ExtData, opt => opt.MapFrom(c => JsonConvert.DeserializeObject<dynamic>(c.ExtensionData)))  在DTO的属性中做了处理
            // .ForMember(c => c.Children, opt => opt.Ignore())
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

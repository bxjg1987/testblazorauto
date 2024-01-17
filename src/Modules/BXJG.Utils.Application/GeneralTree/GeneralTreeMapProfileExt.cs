using AutoMapper;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoMapper
{
    /// <summary>
    /// 经过测试,在有泛型的场景中AutoMapper使用继承并不能达到预期效果
    /// </summary>
    public static class GeneralTreeMapProfileExt
    {
        [Obsolete]
        public static IMappingExpression DtoToEntity(this IMappingExpression config)
        {
            return config;

            return config.ForMember("Code", opt => opt.Ignore())
                    .ForMember("Parent", opt => opt.Ignore())
                    .ForMember("TenantId", opt => opt.Ignore())
                    .ForMember("Children", opt => opt.Ignore())
                    .ForMember("ExtensionData", opt => opt.Ignore())
                    .ForMember("IsDeleted", opt => opt.Ignore())
                    .ForMember("DeleterUserId", opt => opt.Ignore())
                    .ForMember("DeletionTime", opt => opt.Ignore())
                    .ForMember("LastModificationTime", opt => opt.Ignore())
                    .ForMember("LastModifierUserId", opt => opt.Ignore())
                    .ForMember("CreationTime", opt => opt.Ignore())
                    .ForMember("CreatorUserId", opt => opt.Ignore());
        }

        [Obsolete]
        public static IMappingExpression<TSource, TDestination> EntityToDto<TSource, TDestination>(this IMappingExpression<TSource, TDestination> config)
           where TSource : GeneralTreeEntity<TSource>
           where TDestination : GeneralTreeGetTreeNodeBaseDto<TDestination>
        {
            return config;

           // return config.ForMember(c => c.ExtData, opt => opt.Ignore());
        }

        [Obsolete]
        public static IMappingExpression EntityToDto(this IMappingExpression config)
        {
            return config;

            return config.ForMember("ExtData", opt => opt.Ignore());
        }

        public static IMappingExpression<TSource, TDestination> EntityToComboTree<TSource, TDestination>(this IMappingExpression<TSource, TDestination> config)
            where TSource : GeneralTreeEntity<TSource>
            where TDestination : GeneralTreeNodeDto<TDestination>
        {
            return config.ForMember(c => c.Text, opt => opt.MapFrom(c => c.DisplayName));

            return config.ForMember(c => c.Text, opt => opt.MapFrom(c => c.DisplayName))
                    .ForMember(c => c.IconCls, opt => opt.Ignore())
                    .ForMember(c => c.Checked, opt => opt.Ignore());
            //.ForMember(c => c.State, opt => opt.Ignore())
            //.ForMember(c => c.ExtData, opt => opt.Ignore())
        }

        public static IMappingExpression EntityToComboTree(this IMappingExpression config)
        {
            return config.ForMember("Text", opt => opt.MapFrom("DisplayName"));

            return config.ForMember("Text", opt => opt.MapFrom("DisplayName"))
                    .ForMember("IconCls", opt => opt.Ignore())
                    .ForMember("Checked", opt => opt.Ignore())
            //.ForMember(c => c.State, opt => opt.Ignore())
            .ForMember("ExtData", opt => opt.Ignore());
        }

        public static IMappingExpression<TSource, TDestination> EntityToCombobox<TSource, TDestination>(this IMappingExpression<TSource, TDestination> config)
            where TSource : GeneralTreeEntity<TSource>
            where TDestination : GeneralTreeComboboxDto
        {
            return config.ForMember(c => c.DisplayText, opt => opt.MapFrom(c => c.DisplayName))
                    .ForMember(c => c.Value, opt => opt.MapFrom(c => c.Id.ToString()));
        }
     
        public static IMappingExpression EntityToCombobox(this IMappingExpression config)
        {
            return config.ForMember("DisplayText", opt => opt.MapFrom("DisplayName"))
                      .ForMember("Value", opt => opt.MapFrom("Id"));
            //.ForMember(c => c.State, opt => opt.Ignore())
            //.ForMember(c => c.ExtData, opt => opt.Ignore())
        }
    }
}

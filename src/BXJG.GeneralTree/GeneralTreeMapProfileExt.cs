using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.GeneralTree
{
    /// <summary>
    /// 经过测试,在有泛型的场景中AutoMapper使用继承并不能达到预期效果
    /// </summary>
    public static class GeneralTreeMapProfileExt
    {
        public static void EntityToDto<TSource, TDestination>(this IMappingExpression<TSource, TDestination> config)
           where TSource : GeneralTreeEntity<TSource>
           where TDestination : GeneralTreeGetTreeNodeBaseDto<TDestination>
        {
            config.ForMember(c => c.ExtData, opt => opt.Ignore());
        }

        public static void EntityToDto(this IMappingExpression config)
        {
            config.ForMember("ExtData", opt => opt.Ignore());
        }

        public static void EntityToComboTree<TSource, TDestination>(this IMappingExpression<TSource, TDestination> config)
            where TSource : GeneralTreeEntity<TSource>
            where TDestination : GeneralTreeNodeDto<TDestination>
        {
            config.ForMember(c => c.Text, opt => opt.MapFrom(c => c.DisplayName))
                  .ForMember(c => c.IconCls, opt => opt.Ignore())
                  .ForMember(c => c.Checked, opt => opt.Ignore());
            //.ForMember(c => c.State, opt => opt.Ignore())
            //.ForMember(c => c.ExtData, opt => opt.Ignore())
        }

        public static void EntityToComboTree(this IMappingExpression config)
        {
            config.ForMember("Text", opt => opt.MapFrom("DisplayName"))
                  .ForMember("IconCls", opt => opt.Ignore())
                  .ForMember("Checked", opt => opt.Ignore());
            //.ForMember(c => c.State, opt => opt.Ignore())
            //.ForMember(c => c.ExtData, opt => opt.Ignore())
        }



        public static void EntityToCombobox<TSource, TDestination>(this IMappingExpression<TSource, TDestination> config)
            where TSource : GeneralTreeEntity<TSource>
            where TDestination : GeneralTreeComboboxDto
        {
            config.ForMember(c => c.DisplayText, opt => opt.MapFrom(c => c.DisplayName))
                  .ForMember(c => c.Value, opt => opt.MapFrom(c => c.Id.ToString()));
        }

        public static void EntityToCombobox(this IMappingExpression config)
        {
            config.ForMember("DisplayText", opt => opt.MapFrom("DisplayName"))
                  .ForMember("Value", opt => opt.MapFrom("Id"));
            //.ForMember(c => c.State, opt => opt.Ignore())
            //.ForMember(c => c.ExtData, opt => opt.Ignore())
        }
    }
}

using Abp;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Localization;
using Abp.Localization.Sources;
using BXJG.Common.Dto;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXJG.Utils
{
    /// <summary>
    /// 各个模块内都有自己的枚举，各个模块应该提供自己的应用服务向前端提供枚举下拉框的值
    /// <br />这些应用服务可以使用此类来简化列表的生成
    /// </summary>
    public class EnumManager
    {
        private ILocalizationSource localizationSource;

        public EnumManager(ILocalizationManager localizationManager, string localizationSourceName)
        {
            localizationSource = localizationManager.GetSource(localizationSourceName);
        }

        public EnumManager(ILocalizationSource localizationSource)
        {
            this.localizationSource = localizationSource;
        }

        public List<ComboboxItemDto> ConvertToComboboxData<T>(GetForSelectInput input) where T:Enum
        {
            var list = localizationSource.GetEnum<T>();
            var b = list.Select(c => new ComboboxItemDto { Value = c.Key.ToString(), DisplayText = c.Value }).ToList();

            if (input.ForType == 0)
                return b;

            var temp = new ComboboxItemDto { Value = null };
            //列表页 不限 类型名    表单页  请选择  
            if (input.ForType > 0 && !input.ParentText.IsNullOrWhiteSpace())
            {
                temp.DisplayText = localizationSource.GetStringOrNull(input.ParentText);
                if (temp.DisplayText.IsNullOrWhiteSpace())
                    temp.DisplayText = input.ParentText;
            }
            else if (input.ForType == 1)
            {
                temp.DisplayText = localizationSource.GetForType<T>();
            }
            else if (input.ForType == 2)
            {
                temp.DisplayText = "==不限==".UtilsL();
            }
            else
            {
                temp.DisplayText = "==请选择==".UtilsL();
            }
            b.Insert(0, temp);
            return b;
        }
    }
} 

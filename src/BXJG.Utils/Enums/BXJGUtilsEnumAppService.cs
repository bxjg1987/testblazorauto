using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
//using Microsoft.International.Converters.PinYinConverter;
using BXJG.Utils.Localization;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace BXJG.Utils.Enums
{
    public class BXJGUtilsEnumAppService : BXJGUtilsAppServiceBase, IBXJGUtilsEnumAppService
    {
        //IList<ComboboxDto<int>> GetByType(Type t)
        //{
        //    var dic = base.LocalizationSource.EnumToDic(t);
        //    return dic.Select(c => new ComboboxDto<int>(c.Key, c.Value)).ToList();
        //}
        //IList<ComboboxDto<int>> GetByEnumValue(Enum e)
        //{
        //    return GetByType(e.GetType());
        //}
        private BXJGUtilsModuleConfig cfg;
        public BXJGUtilsEnumAppService(BXJGUtilsModuleConfig cfg)
        {
            this.cfg = cfg;
        }

        public IList<ComboboxItemDto> GetByName(GetEnumForCombboxInput input)
        {
            var cfgItem = cfg.Enums.Single(c => c.Name.Equals(input.EnumTypeName, StringComparison.OrdinalIgnoreCase));
            var t = cfgItem.Type;

            IEnumerable<ComboboxItemDto> a;
            if (input.Nullable)
            {
                var list = base.LocalizationSource.GetNullableEnum(t);
                a = list.Select(c => new ComboboxItemDto { Value = c.Key.ToString(), DisplayText = c.Value });
            }
            else
            {
                var list = base.LocalizationSource.GetEnum(t);
                a = list.Select(c => new ComboboxItemDto { Value = c.Key.ToString(), DisplayText = c.Value });
            }
            var b = a.ToList();

            if (input.ForType == 0)
                return b;

            var temp = new ComboboxItemDto { Value = null };

            if (input.ForType > 0 && !input.ParentText.IsNullOrWhiteSpace())
            {
                temp.DisplayText = base.LocalizationManager.GetSource(cfgItem.LocationSourceName).GetStringOrNull(input.ParentText);
                if (temp.DisplayText.IsNullOrWhiteSpace())
                    temp.DisplayText = L(input.ParentText);
            }
            else if (input.ForType == 1)
            {
                b.Insert(0, new ComboboxItemDto(null, base.LocalizationManager.GetForType(cfgItem.LocationSourceName, t)));
                return b;
            }
            if (input.ForType == 3)
            {
                b.Insert(0, new ComboboxItemDto(null, L("==请选择==")));
                return b;
            }

            b.Insert(0, temp);
            return b;
        }

        //public IList<ComboboxDto<int>> GetGender(NullableInput b)
        //{
        //    var lsit = GetByType(typeof(ZLJ.Enums.Gender));
        //    return sdf(lsit,b.Nullable);
        //}

        //public IList<ComboboxDto<int>> GetOUType(NullableInput b)
        //{
        //    var lsit = GetByType(typeof(ZLJ.Enums.OUType));
        //    return sdf(lsit, b.Nullable);
        //}
        //public IList<ComboboxDto<int>> GetWeekDay(NullableInput b)
        //{
        //    var lsit = GetByType(typeof(DayOfWeek));
        //    return sdf(lsit, b.Nullable);
        //}
        //public IList<ComboboxDto<int>> GetAdministrativeLevel(NullableInput b)
        //{
        //    var lsit = GetByType(typeof(XZQ));
        //    return sdf(lsit, b.Nullable);
        //}

        //public string GetPY(GetPYInput input)
        //{
        //    if (!input.Full)
        //    {
        //        string shortR = "";
        //        foreach (char c in input.Chinese.Trim())
        //        {
        //            try
        //            {
        //                ChineseChar chineseChar = new ChineseChar(c);
        //                shortR += chineseChar.Pinyins[0].Substring(0, 1);
        //            }
        //            catch
        //            {
        //                shortR += c;
        //            }

        //        }
        //        return input.ToUpper ? shortR.ToUpper() : shortR;
        //    }

        //    string allR = "";
        //    foreach (char c in input.Chinese.Trim())
        //    {
        //        try
        //        {
        //            ChineseChar chineseChar = new ChineseChar(c);
        //            allR += chineseChar.Pinyins[0].Substring(0, chineseChar.Pinyins[0].Length - 1);
        //        }
        //        catch
        //        {
        //            allR += c;
        //        }
        //    }
        //    return input.ToUpper ? allR.ToUpper() : allR;
        //}


        //public IList<ComboboxDto<int>> GetBool(NullableInput nullable)
        //{
        //    var list = base.LocalizationSource.GetBool().Select(c => new ComboboxDto<int>(c.Key, c.Value)).ToList();
        //    return sdf(list, nullable.Nullable);
        //}

        //IList<ComboboxDto<int>> sdf(IList<ComboboxDto<int>> k,bool b)
        //{
        //    if ( b)
        //        k.Insert(0,new ComboboxDto<int>(-1, L("Unknown")));
        //    return k;
        //}
    }
}

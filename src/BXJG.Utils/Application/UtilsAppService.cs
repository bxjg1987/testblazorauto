using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Dto;
using System.Reflection;
using ZLJ.Localization;
using Microsoft.International.Converters.PinYinConverter;
using ZLJ.Utils.Dto;
using ZLJ.Enums;

namespace ZLJ.Utils
{
    public class UtilsAppService : ABPAppServiceBase, IUtilsAppService
    {
        IList<ComboboxDto<int>> GetByType(Type t)
        {
            var dic = base.LocalizationSource.EnumToDic(t);
            return dic.Select(c => new ComboboxDto<int>(c.Key, c.Value)).ToList();
        }
        IList<ComboboxDto<int>> GetByEnumValue(Enum e)
        {
            return GetByType(e.GetType());
        }

        public IList<ComboboxDto<int>> GetByName(GetEnumByNameInput input)
        {
            Assembly assembly = Assembly.Load("ZLJ.Core");
            var t = assembly.GetType("ZLJ.Enums." + input.EnumTypeName, true, true);
            var list = GetByType(t);
            return sdf(list, input.Nullable);
        }
        public IList<ComboboxDto<int>> GetGender(NullableInput b)
        {
            var lsit = GetByType(typeof(ZLJ.Enums.Gender));
            return sdf(lsit,b.Nullable);
        }

        public IList<ComboboxDto<int>> GetOUType(NullableInput b)
        {
            var lsit = GetByType(typeof(ZLJ.Enums.OUType));
            return sdf(lsit, b.Nullable);
        }
        public IList<ComboboxDto<int>> GetWeekDay(NullableInput b)
        {
            var lsit = GetByType(typeof(DayOfWeek));
            return sdf(lsit, b.Nullable);
        }
        public IList<ComboboxDto<int>> GetAdministrativeLevel(NullableInput b)
        {
            var lsit = GetByType(typeof(XZQ));
            return sdf(lsit, b.Nullable);
        }

        public string GetPY(GetPYInput input)
        {
            if (!input.Full)
            {
                string shortR = "";
                foreach (char c in input.Chinese.Trim())
                {
                    try
                    {
                        ChineseChar chineseChar = new ChineseChar(c);
                        shortR += chineseChar.Pinyins[0].Substring(0, 1);
                    }
                    catch
                    {
                        shortR += c;
                    }

                }
                return input.ToUpper ? shortR.ToUpper() : shortR;
            }

            string allR = "";
            foreach (char c in input.Chinese.Trim())
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(c);
                    allR += chineseChar.Pinyins[0].Substring(0, chineseChar.Pinyins[0].Length - 1);
                }
                catch
                {
                    allR += c;
                }
            }
            return input.ToUpper ? allR.ToUpper() : allR;
        }


        public IList<ComboboxDto<int>> GetBool(NullableInput nullable)
        {
            var list = base.LocalizationSource.GetBool().Select(c => new ComboboxDto<int>(c.Key, c.Value)).ToList();
            return sdf(list, nullable.Nullable);
        }

        IList<ComboboxDto<int>> sdf(IList<ComboboxDto<int>> k,bool b)
        {
            if ( b)
                k.Insert(0,new ComboboxDto<int>(-1, L("Unknown")));
            return k;
        }
    }
}

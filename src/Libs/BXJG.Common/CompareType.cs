using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common
{
    /// <summary>
    /// 常用数据比较符
    /// </summary>
    public enum CompareType
    {
        Dayu = 1 << 0,
        Dengyu = 1 << 1,
        Xiaoyu = 1 << 2,
        DayuDengyu = 1 << 3,
        XiaoyuDengyu = 1 << 4,
        BuDengyu = 1 << 5,

        Baohan = 1 << 6,
        BuBaohan = 1 << 7,
        StartWith = 1 << 8,
        EndWith = 1 << 9,

        Kong = 1 << 10,
        Feikong = 1 << 11
    }
    /// <summary>
    /// 字段类型到可用比较符的映射
    /// </summary>
    public static class CompareTypeMap //:Dictionary<string, CompareType[]>
    {
        private static readonly IReadOnlyDictionary<string, IReadOnlySet<CompareType>> Items = new Dictionary<string, IReadOnlySet<CompareType>>
        {
            {
                "string",
                new HashSet <CompareType>
                {
                    CompareType.Dengyu,
                    CompareType.BuDengyu,
                    CompareType.Baohan,
                    CompareType.BuBaohan,
                    CompareType.StartWith,
                    CompareType.EndWith,
                    CompareType.Kong,
                    CompareType.Feikong
                }
            },
            {
                //数字、时间等，可以做范围比较的
                "val",
                new HashSet <CompareType>
                {
                    CompareType.Dengyu,
                    CompareType.BuDengyu,
                    CompareType.Dayu,
                    CompareType.Xiaoyu,
                    CompareType.DayuDengyu,
                    CompareType.XiaoyuDengyu,
                    //CompareType.Kong,
                    //CompareType.Feikong
                }
            },
            {
                "bool",
                new HashSet <CompareType>
                {
                    CompareType.Dengyu,
                    CompareType.BuDengyu,
                    //CompareType.Kong,
                    //CompareType.Feikong
                }
            }
        };

        //public static readonly IReadOnlyDictionary<Type, IReadOnlySet<CompareType>> Maps = new Dictionary<Type, IReadOnlySet<CompareType>>
        //{
        //    //基元类型名称本就不会重复，没必要用FullName
        //    { typeof(string), Items["string"] },
        //    { typeof(Guid), Items["string"] },
        //    { typeof(int), Items["shu"] },
        //    { typeof(uint), Items["shu"] },
        //    { typeof(double), Items["shu"] },
        //    { typeof(float), Items["shu"] },
        //    { typeof(decimal), Items["shu"] },
        //    { typeof(DateTime), Items["shu"] },
        //    { typeof(DateOnly), Items["shu"] },
        //    { typeof(TimeOnly), Items["shu"] },
        //    { typeof(long), Items["shu"] },
        //    { typeof(ulong), Items["shu"] },
        //    { typeof(byte), Items["shu"] },
        //    { typeof(sbyte), Items["shu"] },
        //    { typeof(short), Items["shu"] },
        //    { typeof(ushort), Items["shu"] },
        //    { typeof(bool), Items["bool"] },
        //    //{ typeof(Guid?), Items["string"] },
        //    //{ typeof(int?), Items["shu"] },
        //    //{ typeof(uint?), Items["shu"] },
        //    //{ typeof(double?), Items["shu"] },
        //    //{ typeof(float?), Items["shu"] },
        //    //{ typeof(decimal?), Items["shu"] },
        //    //{ typeof(DateTime?), Items["shu"] },
        //    //{ typeof(DateOnly?), Items["shu"] },
        //    //{ typeof(TimeOnly?), Items["shu"] },
        //    //{ typeof(long?), Items["shu"] },
        //    //{ typeof(ulong?), Items["shu"] },
        //    //{ typeof(byte?), Items["shu"] },
        //    //{ typeof(sbyte?), Items["shu"] },
        //    //{ typeof(short?), Items["shu"] },
        //    //{ typeof(ushort?), Items["shu"] },

        //    //{ typeof(bool?), Items["bool"] },

        //    //{ typeof(Enum), Items["boolOrEnum"] },
        //    //{ typeof(Nullable<Enum>), Items["boolOrEnum"] },
        //};

        /// <summary>
        /// 获取指定类型可以使用的比较符
        /// </summary>
        /// <param name="typeName">类型短名称，即：typeof(xxx).Name，内部自动忽略大小写</param>
        /// <param name="nullabel">此类型是否可控</param>
        /// <returns></returns>
        public static IReadOnlySet<CompareType> GetCompareTypes(string typeName, bool nullabel = true)
        {
            HashSet<CompareType> hs;
            typeName = typeName.ToLower();

            if (typeName == typeof(bool).Name)
            {
                hs = Items[typeof(bool).Name].ToHashSet();
            }
            else if (typeName == "enum")
            {
                hs = Items["bool"].ToHashSet();
            }
            else if (typeName == typeof(string).Name)
            {
                hs = Items[typeof(string).Name].ToHashSet();
                nullabel = false;
            }
            else
            {
                hs = Items["val"].ToHashSet();
            }

            if (nullabel)
            {
                hs.Add(CompareType.Kong);
                hs.Add(CompareType.Feikong);
            }

            return hs;
        }
        /// <summary>
        /// 获取指定类型可以使用的比较符
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public static IReadOnlySet<CompareType> GetCompareTypes<T>()
        {
            return GetCompareTypes(typeof(T));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IReadOnlySet<CompareType> GetCompareTypes(Type t)
        {
            var typeName = t.Name;

            if (t.IsEnum)
            {
                typeName = "enum";
            }
            //var nullable = t.IsNullable();

            return GetCompareTypes(typeName, t.IsNullable());
        }


    }

    //从mudblazor的动态条件部分复制过来的
    //internal class TypeIdentifier
    //{
    //    private static readonly HashSet<Type> _numericTypes = new HashSet<Type>
    //    {
    //        typeof(int),
    //        typeof(double),
    //        typeof(decimal),
    //        typeof(long),
    //        typeof(short),
    //        typeof(sbyte),
    //        typeof(byte),
    //        typeof(ulong),
    //        typeof(ushort),
    //        typeof(uint),
    //        typeof(float),
    //        typeof(BigInteger),
    //        typeof(int?),
    //        typeof(double?),
    //        typeof(decimal?),
    //        typeof(long?),
    //        typeof(short?),
    //        typeof(sbyte?),
    //        typeof(byte?),
    //        typeof(ulong?),
    //        typeof(ushort?),
    //        typeof(uint?),
    //        typeof(float?),
    //        typeof(BigInteger?)
    //    };

    //    internal static bool IsString(Type? type)
    //    {
    //        if ((object)type == null)
    //        {
    //            return false;
    //        }

    //        if (type == typeof(string))
    //        {
    //            return true;
    //        }

    //        return false;
    //    }

    //    public static bool IsNumber(Type? type)
    //    {
    //        if ((object)type != null)
    //        {
    //            return _numericTypes.Contains(type);
    //        }

    //        return false;
    //    }

    //    public static bool IsEnum(Type? type)
    //    {
    //        if ((object)type == null)
    //        {
    //            return false;
    //        }

    //        if (type!.IsEnum)
    //        {
    //            return true;
    //        }

    //        return Nullable.GetUnderlyingType(type)?.IsEnum ?? false;
    //    }

    //    public static bool IsDateTime(Type? type)
    //    {
    //        if ((object)type == null)
    //        {
    //            return false;
    //        }

    //        if (type == typeof(DateTime))
    //        {
    //            return true;
    //        }

    //        Type underlyingType = Nullable.GetUnderlyingType(type);
    //        if ((object)underlyingType != null)
    //        {
    //            return underlyingType == typeof(DateTime);
    //        }

    //        return false;
    //    }

    //    public static bool IsBoolean(Type? type)
    //    {
    //        if ((object)type == null)
    //        {
    //            return false;
    //        }

    //        if (type == typeof(bool))
    //        {
    //            return true;
    //        }

    //        Type underlyingType = Nullable.GetUnderlyingType(type);
    //        if ((object)underlyingType != null)
    //        {
    //            return underlyingType == typeof(bool);
    //        }

    //        return false;
    //    }

    //    public static bool IsGuid(Type? type)
    //    {
    //        if ((object)type == null)
    //        {
    //            return false;
    //        }

    //        if (type == typeof(Guid))
    //        {
    //            return true;
    //        }

    //        Type underlyingType = Nullable.GetUnderlyingType(type);
    //        if ((object)underlyingType != null)
    //        {
    //            return underlyingType == typeof(Guid);
    //        }

    //        return false;
    //    }
    //}
    //public class FieldType
    //{
    //    public Type? InnerType { get; init; }

    //    public bool IsString { get; init; }

    //    public bool IsNumber { get; init; }

    //    public bool IsEnum { get; init; }

    //    public bool IsDateTime { get; init; }

    //    public bool IsBoolean { get; init; }

    //    public bool IsGuid { get; init; }

    //    public static FieldType Identify(Type? type)
    //    {
    //        return new FieldType
    //        {
    //            InnerType = type,
    //            IsString = TypeIdentifier.IsString(type),
    //            IsNumber = TypeIdentifier.IsNumber(type),
    //            IsEnum = TypeIdentifier.IsEnum(type),
    //            IsDateTime = TypeIdentifier.IsDateTime(type),
    //            IsBoolean = TypeIdentifier.IsBoolean(type),
    //            IsGuid = TypeIdentifier.IsGuid(type)
    //        };
    //    }
    //}
}

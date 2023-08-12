using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common
{
    /// <summary>
    /// 通用比较符号
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
        public static readonly IReadOnlyDictionary<string, IReadOnlySet<CompareType>> Items = new Dictionary<string, IReadOnlySet<CompareType>>
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
                "val",
                new HashSet <CompareType>
                {
                    CompareType.Dengyu,
                    CompareType.BuDengyu,
                    CompareType.Dayu,
                    CompareType.Xiaoyu,
                    CompareType.DayuDengyu,
                    CompareType.XiaoyuDengyu,
                    CompareType.Kong,
                    CompareType.Feikong
                }
            },
            {
                "bool",
                new HashSet <CompareType>
                {
                    CompareType.Dengyu,
                    CompareType.BuDengyu,
                    CompareType.Kong,
                    CompareType.Feikong
                }
            }
        };
        public static readonly IReadOnlyDictionary<string, IReadOnlySet<CompareType>> Maps = new Dictionary<string, IReadOnlySet<CompareType>>
        {
            //基元类型名称本就不会重复，没必要用FullName
            { typeof(string).Name, Items["string"] },
            { typeof(Guid).Name, Items["string"] },
            { typeof(int).Name, Items["shu"] },
            { typeof(uint).Name, Items["shu"] },
            { typeof(double).Name, Items["shu"] },
            { typeof(float).Name, Items["shu"] },
            { typeof(decimal).Name, Items["shu"] },
            { typeof(DateTime).Name, Items["shu"] },
            { typeof(DateOnly).Name, Items["shu"] },
            { typeof(TimeOnly).Name, Items["shu"] },
            { typeof(long).Name, Items["shu"] },
            { typeof(ulong).Name, Items["shu"] },
            { typeof(byte).Name, Items["shu"] },
            { typeof(sbyte).Name, Items["shu"] },
            { typeof(short).Name, Items["shu"] },
            { typeof(ushort).Name, Items["shu"] },

            { typeof(Guid?).Name, Items["string"] },
            { typeof(int?).Name, Items["shu"] },
            { typeof(uint?).Name, Items["shu"] },
            { typeof(double?).Name, Items["shu"] },
            { typeof(float?).Name, Items["shu"] },
            { typeof(decimal?).Name, Items["shu"] },
            { typeof(DateTime?).Name, Items["shu"] },
            { typeof(DateOnly?).Name, Items["shu"] },
            { typeof(TimeOnly?).Name, Items["shu"] },
            { typeof(long?).Name, Items["shu"] },
            { typeof(ulong?).Name, Items["shu"] },
            { typeof(byte?).Name, Items["shu"] },
            { typeof(sbyte?).Name, Items["shu"] },
            { typeof(short?).Name, Items["shu"] },
            { typeof(ushort?).Name, Items["shu"] },
        };

        public static IReadOnlySet<CompareType> GetCompareTypes(string typeName) => Maps[typeName];
        public static IReadOnlySet<CompareType> GetCompareTypes<T>() => Maps[typeof(T).Name];
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System
{
    public static class ObjectExtensions
    {
        public static void SetFieldOrPropertyValue(this object obj, string propertyName, object value, BindingFlags flag = BindingFlags.NonPublic| BindingFlags.Public| BindingFlags.IgnoreCase | BindingFlags.Instance)
        {
            var t = obj.GetType();
            var prop = t.GetProperty(propertyName, flag);
            var field = t.GetField(propertyName, flag);
            if (prop == null && field == null)
                throw new ArgumentException($"类型 {t.Name} 上不存在属性或字段 '{propertyName}'");
            Type fieldOrPropertyType;
            if (prop != default)
                fieldOrPropertyType = prop.PropertyType;
            else
                fieldOrPropertyType = field.FieldType;

            //if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            //{
            //  var sdf = new NullableConverter.StandardValuesCollection(prop.PropertyType.GetGenericArguments())
            //    dynamic objValue = System.Activator.CreateInstance(prop.PropertyType);
            //    objValue = value;
            //    prop.SetValue(obj, (object)objValue, null);
            //}
            //else
            //{
            //    prop.SetValue(obj, value, null);
            //}
            object convertedValue = value;
            if (value != null && value.GetType() != fieldOrPropertyType)
            {
                //PropertyDescriptor ss;
                // ss.Converter.string
                Type propertyType = Nullable.GetUnderlyingType(fieldOrPropertyType) ?? fieldOrPropertyType;

                var converter = TypeDescriptor.GetConverter(propertyType);
                convertedValue = converter.ConvertFrom(value); //converter.ConvertFromInvariantString(value.ToString());
                //  converter.ConvertFromString()
                // convertedValue =  Convert.ChangeType(value, propertyType);
            }
            if (prop != default)
                prop.SetValue(obj, convertedValue, null);
            else
                field.SetValue(obj, convertedValue);
            //  prop.SetValue(obj, Convert.ChangeType( value,prop.PropertyType) ,null);
        }

        /// <summary>
        /// 反射获取指定属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static object GetFieldOrPropertyValue(this object obj, string propertyName, BindingFlags flag = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance)
        {
            var t = obj.GetType();
            var p = t.GetProperty(propertyName, flag);
            if (p != default)
                return p.GetValue(obj, null);
            var f = t.GetField(propertyName, flag);
            if (f == null)
                throw new ArgumentException($"类型 {t.Name} 上不存在属性或字段 '{propertyName}'");
            return f.GetValue(obj);
        }
        /// <summary>
        /// 递归获取指定字段
        /// 默认的GetField好像不会递归父类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static FieldInfo GetPrivateField(this Type type, string fieldName)
        {
            while (type != null)
            {
                var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null) return field;
                type = type.BaseType;
            }
            return null;

        }

        public static object? GetFieldValue(this object obj, string fieldName)
        {
            var type = obj.GetType();
            var p = type.GetPrivateField(fieldName); 
            if(p!=null)
                return p.GetValue(obj);
            return null;
        }

        /// <summary>
        /// 反射获取对象属性值并转换为指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static T GetFieldOrPropertyValue<T>(this object obj, string propertyName, BindingFlags flag = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance)
        {
            var value = obj.GetFieldOrPropertyValue(propertyName, flag);
            return (T)Convert.ChangeType(value, typeof(T));
        }
        /// <summary>
        /// 反射获取对象属性并转换为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetValueString(this object obj, string propertyName)
        {
            return obj.GetFieldOrPropertyValue<string>(propertyName);
        }
        /// <summary>
        /// 尝试做减法运算
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj2"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool TrySubtract(this object obj, object obj2, out object val)
        {
            try
            {
                var p = Convert.ToDecimal(obj);
                var p1 = Convert.ToDecimal(obj2);
                val = p - p1;
                return true;
            }
            catch
            {
                val = default;
                return false;
            }
        }
        /// <summary>
        /// 尝试做减法运算
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj2"></param>
        /// <param name="val">失败时的默认值</param>
        /// <returns></returns>
        public static object TrySubtract(this object obj, object obj2, object val = default)
        {
            if (obj.TrySubtract(obj2, out var p))
                return p;

            return val;
        }

        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            var t = obj.GetType();
            var ps = t.GetProperties(BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
            var dic = new Dictionary<string, object>();
            foreach (var p in ps)
            {
                dic.Add(p.Name, p.GetValue(obj, null));
            }
            return dic;
        }


        #region 临时改变对象状态
        //如果使用深拷贝，则范围内可以修改任意状态，但目标对象若是个非常复杂的对象，而范围内只修改极少的数据时，太浪费了。
        private class PropertySnapshot : IDisposable
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            object target;
            public PropertySnapshot(object obj, params string[] ms)
            {
                target = obj;
                foreach (var item in ms)
                {
                    dic.Add(item, obj.GetFieldOrPropertyValue(item));
                }
            }

            public void Dispose()
            {
                foreach (var item in dic)
                {
                    target.SetFieldOrPropertyValue(item.Key, item.Value);
                }
                dic.Clear();
            }
        }

        public static IDisposable TemporarilySet(this object obj, params string[] ms)
        {
            return new PropertySnapshot(obj, ms);
        }
        #endregion

        [return: NotNullIfNotNull(nameof(obj))]
        public static T? DeepClone<T>(this T? obj)
        {
            return FastCloner.FastCloner.DeepClone(obj);
        }
    }
}
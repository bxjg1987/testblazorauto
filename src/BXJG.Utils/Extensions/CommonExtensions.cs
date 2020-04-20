using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.Extensions
{
    //之前这些扩展方法都是按类型分开的，现在打算用CommonExtensions统一定义

    public static class CommonExtensions
    {
        #region 非空验证
        /// <summary>
        /// 若为空，则报ArgumentNullException异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="parameterName"></param>
        public static void RequiredValidate<T>(this Nullable<T> val, string parameterName = "")
            where T : struct
        {
            if (!val.HasValue)
                throw new ArgumentNullException(parameterName);
        }
        /// <summary>
        /// 若为空，则报ArgumentNullException异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="parameterName"></param>
        public static void RequiredValidate(this object val, string parameterName = "")
        {
            if (val == null)
                throw new ArgumentNullException(parameterName);
        }
        /// <summary>
        /// 若为空，则报ArgumentNullException异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="parameterName"></param>
        public static void RequiredValidate(this string str, string parameterName="")
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentNullException(parameterName);
        }
        /// <summary>
        /// 若为空，则报ArgumentNullException异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="parameterName"></param>
        public static void RequiredValidate(this int? val, string parameterName = "")
        {
            RequiredValidate<int>(val, parameterName);
        }
        /// <summary>
        /// 若为空，则报ArgumentNullException异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="parameterName"></param>
        public static void RequiredValidate(this DateTime? val, string parameterName = "")
        {
            RequiredValidate<DateTime>(val, parameterName);
        }
        /// <summary>
        /// 若为空，则报ArgumentNullException异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="parameterName"></param>
        public static void RequiredValidate(this DateTimeOffset? val, string parameterName = "")
        {
            RequiredValidate<DateTimeOffset>(val, parameterName);
        }
        /// <summary>
        /// 若为空，则报ArgumentNullException异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="parameterName"></param>
        public static void RequiredValidate(this decimal? val, string parameterName = "")
        {
            RequiredValidate<decimal>(val, parameterName);
        }
        /// <summary>
        /// 若为空，则报ArgumentNullException异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="parameterName"></param>
        public static void RequiredValidate(this long? val, string parameterName = "")
        {
            RequiredValidate<long>(val, parameterName);
        }
        /// <summary>
        /// 若为空，则报ArgumentNullException异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="parameterName"></param>
        public static void RequiredValidate(this bool? val, string parameterName = "")
        {
            RequiredValidate<bool>(val, parameterName);
        }
        /// <summary>
        /// 若为空，则报ArgumentNullException异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="parameterName"></param>
        public static void RequiredValidate(this Array val, string parameterName = "") {
            if (val == null || val.Length == 0)
                throw new ArgumentNullException(parameterName);
        }
        #endregion

    }
}

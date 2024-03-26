using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Common.Dto
{
    public static class BatchOperationErrorMessageExt
    {
        public static BatchOperationErrorMessage Message500(this object id)
        {
            return new BatchOperationErrorMessage(id, "服务器内部异常");
        }
    }

    /// <summary>
    /// 批量操作错误消息
    /// </summary>
    public class BatchOperationErrorMessage
    {
        public BatchOperationErrorMessage(object id, string message = default, string code = default)
        {
            Id = id;
            Message = message;
            Code = code;
        }

        public object Id { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return $" {Message} （Id：{Id} 代码：{Code}）";
        }

        //public static BatchOperationErrorMessage Message500(object id)
        //{
        //    return new BatchOperationErrorMessage(id, "服务器内部异常");
        //}
    }
    /// <summary>
    /// 批量操作输出模型
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class BatchOperationOutputBase
    {
        /// <summary>
        /// 操作成功的id集合
        /// </summary>
        public IList<object> Ids { get; } = new List<object>();
        /// <summary>
        /// 操作失败的id和原因<br />
        /// 理想的需要如下形式，但abp中 后端和前端都没有针对此方式做适配，因此也可以在错误时封装为UserFriendlyException
        /// </summary>
        public IList<BatchOperationErrorMessage> ErrorMessage { get; } = new List<BatchOperationErrorMessage>();
    }

    /// <summary>
    /// 批量操作输出模型
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class BatchOperationOutput<TKey> : BatchOperationOutputBase
    {
        /// <summary>
        /// 操作成功的id集合
        /// </summary>
        public new IList<TKey> Ids { get; } = new List<TKey>();
        ///// <summary>
        ///// 操作失败的id和原因<br />
        ///// 理想的需要如下形式，但abp中 后端和前端都没有针对此方式做适配，因此也可以在错误时封装为UserFriendlyException
        ///// </summary>
        //public IList<BatchOperationErrorMessage> ErrorMessage { get; } = new List<BatchOperationErrorMessage>();
    }

    public class BatchOperationOutputInt : BatchOperationOutput<int>
    {
    }

    /// <summary>
    /// 批量操作的返回模型，id类型为long
    /// </summary>
    public class BatchOperationOutputLong : BatchOperationOutput<long>
    {
    }
    public class BatchOperationOutputString : BatchOperationOutput<string>
    {
    }
    public class BatchOperationOutputGuid : BatchOperationOutput<Guid>
    {
    }
    public class BatchOperationOutputObject : BatchOperationOutput<object>
    {
    }
}

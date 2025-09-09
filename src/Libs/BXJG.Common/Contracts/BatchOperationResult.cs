using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXJG.Common.Contracts
{
    public static class BatchOperationErrorMessageExt
    {
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 实例化批量操作错误消息
        /// </summary>
        public BatchOperationErrorMessage(object id, string message = default, string code = default)
        {
            Id = id;
            Message = message;
            Code = code;
        }
        /// <summary>
        /// 产生错误的数据的id
        /// </summary>
        public object Id { get; set; }
        /// <summary>
        /// 错误编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return $" {Message} （Id：{Id} 代码：{Code}）";
        }

        //public static BatchOperationErrorMessage Message500(object id)
        //{
        //    return new BatchOperationErrorMessage(id, "服务器内部异常");
        //}
    }


    /*
     * 某些场景中，调用方根本不关心Ids中元素的类型，用BatchOperationOutput更简单。
     * 所以BatchOperationOutput不能是抽象的，必须是直接可用的。
     * 
     * 某些场景调用方又希望指定ids中元素的类型，方便后续使用时有强类型提示，所以使用BatchOperationOutput<TKey>更方便。
     * 
     * 还有些场景希望BatchOperationOutput<TKey>可以顶替BatchOperationOutput，所以它们必须有继承关系，此时最终使用的是子类的ids属性。
     * 
     * 由于ids是读多写少，所以BatchOperationOutput<TKey>中应该存储一份强类型的ids，避免反复装箱/拆箱。
     */

    /// <summary>
    /// 批量操作输出模型
    /// </summary>
    public class BatchOperationOutput
    {
        /// <summary>
        /// 操作成功的id集合
        /// </summary>
        public virtual IList<object> Ids
        {
            get
            {
                return GetIds();
            }
            set {
                _ids = value;
            }
        }
        /// <summary>
        /// 操作失败的id和原因<br />
        /// 理想的需要如下形式，但abp中 后端和前端都没有针对此方式做适配，因此也可以在错误时封装为UserFriendlyException
        /// </summary>
        public virtual IList<BatchOperationErrorMessage> ErrorMessage { get; set; } = new List<BatchOperationErrorMessage>();
        IList<object> _ids = new List<object>();
        protected virtual IList<object> GetIds() => _ids;
    }

    /// <summary>
    /// 批量操作输出模型
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class BatchOperationOutput<TKey> : BatchOperationOutput
    {
        /// <summary>
        /// 操作成功的id集合
        /// </summary>
        public new IList<TKey> Ids { get; set; } = new List<TKey>();
        protected override IList<object> GetIds()
        {
            return Ids.Cast<object>().ToList();
        }
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

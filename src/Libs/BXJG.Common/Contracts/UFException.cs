using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BXJG.Common.Contracts
{
    /// <summary>
    /// 用户友好的异常
    /// </summary>
    public class UFException : Exception
    {
        public UFException()
        {
        }

        public UFException(string message) : base(message)
        {
        }

        public UFException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UFException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

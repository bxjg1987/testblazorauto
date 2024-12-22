using BXJG.Common.Contracts;
using BXJG.Utils.Application.ClientProxy.Http;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.RCL.Exceptions
{
    public class ErrorCallback : IErrorCallback
    {
        IMessageService messageService;
      //  readonly ILogger<ErrorCallback> logger;

        public ErrorCallback(IMessageService messageService/*, ILogger<ErrorCallback> logger*/)
        {
            this.messageService = messageService;
           // this.logger = logger;
        }

        public void Hand(IEnumerable<BatchOperationErrorMessage> errorInfo)
        {
            string message = string.Join("\n", errorInfo.Select(p => p.Message));
          //  logger.LogError("message1111");

            messageService.Error(message);
        }
    }
}

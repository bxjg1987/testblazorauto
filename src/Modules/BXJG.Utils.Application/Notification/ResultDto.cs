using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Notification
{
    public class ResultDto<T>: PagedResultDto<T>
    {
        /// <summary>
        /// 未读消息
        /// </summary>
        public int UnReadCount { get; set; }
        /// <summary>
        /// 已读消息
        /// </summary>
        public int ReadCount { get; set; }
    }
}

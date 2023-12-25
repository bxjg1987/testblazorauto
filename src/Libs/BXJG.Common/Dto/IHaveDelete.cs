using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common.Dto
{
    /// <summary>
    /// 这个定义在应用层dto合适吗？
    /// dto本就是承上启下，为ui服务的，定义了，可用可不用
    /// 要么在外面包一层泛型，有点恶心
    /// 只怪mudblazor的CellContext不可扩展 已经提出请求：https://github.com/MudBlazor/MudBlazor/issues/7456 看看官方会不会处理
    /// </summary>
    public interface IHaveDelete
    {
        /// <summary>
        /// 是否显示删除确认框
        /// </summary>
        public bool IsShowDeleteConfirmation { get; set; }
        /// <summary>
        /// 是否正在删除
        /// </summary>
        public bool IsDeleting { get; set; }
    }

    // public class sdfsdf : IHaveDelete { }
}

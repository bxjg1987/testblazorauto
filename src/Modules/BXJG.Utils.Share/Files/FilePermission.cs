using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Share.Files
{
    /// <summary>
    /// 文件访问权限
    /// 目前的文件上传、删除都是依赖附件的，这里的权限仅仅是说明通用文件权限，也就是通用文件下载接口需要判断的
    /// 将来可能做单独的文件管理接口，那时的权限还是按功能权限来做。
    /// 某些附件需要更严格的权限控制，此时具体业务应该提供单独的文件访问接口，通常是根据具体业务的功能权限来判断
    /// </summary>
    [Flags]
    public enum FilePermission
    {
        /// <summary>
        /// 匿名用户可访问
        /// </summary>
        Anonymous = 1 << 0,
        /// <summary>
        /// 登录的用户才能访问
        /// </summary>
        Authenticated = 1 << 1,
        /// <summary>
        /// 具体业务自己的文件访问接口自己去判断权限，不依赖此字段
        /// </summary>
        Further = 1 << 2,
        /// <summary>
        /// 自己只能访问自己的文件
        /// </summary>
        Owner = 1 << 3,
        /// <summary>
        /// 指定权限字符串
        /// 文件中单独用一个字典存储权限字符串，通过此字符串决定访问权限
        /// </summary>
        PermissionNames = 1 << 4,
    }
}

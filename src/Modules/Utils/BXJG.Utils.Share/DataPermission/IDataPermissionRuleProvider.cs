using System.Collections.Generic;
using System.Threading.Tasks;

namespace BXJG.Utils.Share.DataPermission
{
    /// <summary>
    /// 数据权限规则提供者接口
    /// 用于获取当前用户对指定实体类型的数据权限规则
    /// </summary>
    public interface IDataPermissionRuleProvider
    {
        /// <summary>
        /// 获取当前用户对指定实体类型的数据权限规则
        /// 内部自动解析IAbpSession获取用户ID和租户ID
        /// </summary>
        /// <param name="entityTypeFullName">实体类型全名</param>
        /// <returns>数据权限规则列表</returns>
        DataPermissionDto GetRules(string entityTypeFullName);

        /// <summary>
        /// 异步获取当前用户对指定实体类型的数据权限规则
        /// 内部自动解析IAbpSession获取用户ID和租户ID
        /// </summary>
        /// <param name="entityTypeFullName">实体类型全名</param>
        /// <returns>数据权限规则列表</returns>
        Task<DataPermissionDto> GetRulesAsync(string entityTypeFullName);
    }
}

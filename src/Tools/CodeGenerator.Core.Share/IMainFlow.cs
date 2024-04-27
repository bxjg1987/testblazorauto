using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Core.Share
{
    /*
     * 程序启动后由用户开始操作
     * 选择项目
     * 选择模型
     * 选择模板
     * 选择后续工作
     * 并行执行多个主流程
     * 收尾工作
     * 
     * 多个主流程并行执行
     * 一个主流程内部单线程，且仅做一件事
     * 
     * 不同框架差异太大，主流程的默认实现难以抽离，不过起码有一些共同的步骤：
     * 生成实体
     * 配置仓储映射
     * 执行数据库迁移
     * 生成应用服务
     * 生成接口
     * 定义权限
     * 定义菜单
     * 生成ui
     *      列表
     *      表单
     * 
     * 这些步骤都可以定义成单个主流程并行生成，内部处理都很简单，貌似木有必要回调事件了。
     */

    /// <summary>
    /// 主流程定义
    /// </summary>
    public interface IMainFlow
    {
        ValueTask Execute(MainFlowContext context, CancellationToken cancellationToken = default);
    }
}
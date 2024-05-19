using ZLJ.Application.Share.TestTree;

namespace ZLJ.Admin.CoreRCL.TestTree
{
    /// <summary>
    /// Admin应用 测试树 UI 新增 逻辑
    /// </summary>
    public partial class Create
    {
        /// <summary>
        /// 用于实现Section布局
        /// </summary>
        [Parameter]
        public object Master { get; set; }
        /// <summary>
        /// 此功能的名称
        /// </summary>
        public override string FuncName => "测试树";
    }
}
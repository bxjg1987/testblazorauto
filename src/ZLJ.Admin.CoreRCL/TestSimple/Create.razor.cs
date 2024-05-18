using ZLJ.Application.Share.TestSimple;

namespace ZLJ.Admin.CoreRCL.TestSimple
{
    public partial class Create
    {
        [Parameter]
        public object Master { get; set; }
        public override string FuncName => "普通数据测试";

        //protected override ValueTask ResetCore()
        //{
        //    createDto.Reset();
        //    return ValueTask.CompletedTask;
        //}
    }
}
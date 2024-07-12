namespace ZLJ.Admin.CoreRCL
{
    public partial class Home
    {

        IDisposable getall;
        protected override void OnInitialized()
        {
            getall=  base.Zhongjie.Zhuce(() =>
            {
                base.ShowSuccessMessage("服务端getall接口返回的数据有变化，请刷新页面以获取更新。");
                return ValueTask.CompletedTask;
            }, BXJG.Utils.Application.Share.Consts.ETGetAll);
            base.OnInitialized();
        }

        [AbpExceptionInterceptor]
        void TestException()
        {

            Console.WriteLine("xx测试热重载！");
            throw new Exception("未处理异常！");
        }
        protected override void Dispose(bool disposing)
        {
            getall?.Dispose();
            base.Dispose(disposing);
        }

        private async Task cfqdsj() {
          await  base.HttpClientFactory.CreateBXJGUtils().Post("api/demo/triggergetallapplication", new { id = 0 });
        }
    }
}

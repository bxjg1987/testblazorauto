namespace ZLJ.Admin.CoreRCL
{
    public partial class Home
    {

       

        [AbpExceptionInterceptor]
        void TestException() {

            Console.WriteLine(  "xx测试热重载！");
            throw new Exception("未处理异常！");
        }
    }
}

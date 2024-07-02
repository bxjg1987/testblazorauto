namespace ZLJ.Admin.CoreRCL
{
    public partial class Home
    {

       

        [AbpExceptionInterceptor]
        void TestException() { 
     
            throw new Exception("未处理异常！");
        }
    }
}

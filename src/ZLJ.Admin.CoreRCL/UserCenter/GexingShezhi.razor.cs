using BXJG.Utils.Application.Share.Settings;
namespace ZLJ.Admin.CoreRCL.UserCenter
{
    public partial class GexingShezhi
    {
        Tree<SettingDefinitionGroupDto> tree;
        protected override HttpClient HttpClient => HttpClientFactory.CreateHttpClientUtils();
        /// <summary>
        /// 界面消息服务
        /// </summary>
        [Inject]
        public IMessageService MessageService { get; set; }

        //await消息显示时，好像会等到消息因此时才结束，没严格测试
        //不过测试发现消息异步显示，并等待200毫秒，消息提示更丝滑
        //



        protected override async Task ShowFailMessage(string title = "操作提示", string msg = "操作失败！")
        {
            MessageService.Error(msg);//它是阻塞到显示完成因此元素后，所以不能等待它
            await Task.Delay(300);
        }
        protected override async Task ShowSuccessMessage(string title = "操作提示", string msg = "操作成功！")
        {
            MessageService.Success(msg);//它是阻塞到显示完成因此元素后，所以不能等待它
            await Task.Delay(300);
        }
    }
}

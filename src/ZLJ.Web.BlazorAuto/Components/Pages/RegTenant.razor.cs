using AntDesign;
using ZLJ.Application.Common.Share.MultiTenancy;
using ZLJ.RCL.Interceptors;

namespace ZLJ.Web.BlazorAuto.Components.Pages
{
    public partial class RegTenant
    {
        Dictionary<string,object> pwdAttr = new Dictionary<string, object>() { { "autocomplete", "new-password" } };
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        ResultStatus resultStatus = ResultStatus.Info;
        RegisterTenantInput model = new RegisterTenantInput();
        RegisterTenantOutput jg = new RegisterTenantOutput() { Name = "xxxxxxxxxxxxxxx" };
        [Inject]
        public IConfiguration Configuration { get; set; }
        string msg="";
        string pwd = "***********";
        bool showPassword = false;
        string yzmUrl;
        bool isSubmiting = false;
        protected override HttpClient HttpClient => httpClient ??= base.HttpClientFactory.CreateHttpClientCommon();
        //[AbpExceptionInterceptor]//不晓得为啥无效
        public async Task OnFinish()
        {
            try
            {
                isSubmiting = true;
                //await Task.Delay(3000);
                //throw new Exception("xxxx");
                jg = await HttpClient.Post<RegisterTenantOutput>("TenantRegistration", "RegisterTenant", model, null);
                resultStatus = ResultStatus.Success;
                //await base.ShowSuccessMessage();//不晓得为啥无效
            }
            catch (Exception ex)
            {
                this.msg = ex.Message;
                await base.MessageService.ErrorAsync(ex.Message);
            }
            finally
            {
                isSubmiting = false;
            }
        }
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                YzmClick();
            }
        }

        Input<string> yzm;
        void YzmClick()
        {
            var serverRootAddress = Configuration["App:ServerRootAddress"];
            model.YzmKey = Guid.NewGuid().ToString("n");
            yzmUrl = $"{serverRootAddress}/api/Captcha/Captcha?id={model.YzmKey}&r={Random.Shared.Next(100000)}";
            model.YzmValue = "";
            if(yzm!=default)
            yzm.Focus();
            //    var url = serverRootAddress + "/api/Captcha/Captcha?id=" + sjz;

            // yzmUrl = $"{Configuration["ZLJ:YzmUrl"]}?key={model.YzmKey}";
        }

        void TogglePasswordVisibility()
        {
            showPassword = !showPassword;
        }

        void jrgld() {
            NavigationManager.NavigateTo($"/account/login?tenantName={jg.TenancyName}&userName={jg.UserName}&ReturnUrl=/main", true);
        }
    }
}

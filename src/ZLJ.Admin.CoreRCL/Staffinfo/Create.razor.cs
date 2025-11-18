using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using ZLJ.Application.Share.StaffInfo;

namespace ZLJ.Admin.CoreRCL.Staffinfo
{
    public partial class Create
    {

        //[Parameter]
        //public object Master { get; set; }
        public override string FuncName => "员工";
        protected override Task Save()
        {
     
       
            if (this.createDto.IsEnableAccount &&
               (createDto.Name.IsNullOrWhiteSpaceBXJG() || createDto.Password.IsNullOrWhiteSpaceBXJG()))
            {
                if (createDto.Name.IsNullOrWhiteSpaceBXJG())
                {
                   _= base.ShowFailMessage(msg: "请输入用户名");
                    return Task.CompletedTask;
                }
                if (createDto.Password.IsNullOrWhiteSpaceBXJG() )
                {
                    _ = base.ShowFailMessage(msg: "请输入密码");
                    return Task.CompletedTask;
                }
            }
            return base.Save();
        }
    }
}
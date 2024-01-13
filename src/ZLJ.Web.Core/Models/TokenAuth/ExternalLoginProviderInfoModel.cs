using Abp.AutoMapper;
using ZLJ.Web.Core.Authentication.External;


namespace ZLJ.Web.Core.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}

using System.Collections.Generic;

namespace ZLJ.Web.Core.Authentication.External
{
    public interface IExternalAuthConfiguration
    {
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}

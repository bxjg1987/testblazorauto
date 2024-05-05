using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    public class AppDefine
    {
        public ExecuteContext ExecuteContext;
        public string Name { get; set; }
        public string DisplayName { get; set; }

        string clientProxyProjectName, blazorClientProjectName, applicationProjectName, applicationShareProjectName, blazorHostProjectName, apiHostProjectName;
        public string ClientProxyProjectName
        {
            get { return clientProxyProjectName.IsNotNullOrWhiteSpaceBXJG() ? clientProxyProjectName : $"{ExecuteContext.Name}.{Name}.ClientProxy"; }
            set { clientProxyProjectName = value; }
        }
        public string BlazorClientProjectName
        {
            get { return blazorClientProjectName.IsNotNullOrWhiteSpaceBXJG() ? blazorClientProjectName : $"{ExecuteContext.Name}.{Name}.BlazorClient"; }
            set { blazorClientProjectName = value; }
        }
        public string ApplicationProjectName
        {
            get { return applicationProjectName.IsNotNullOrWhiteSpaceBXJG() ? applicationProjectName : $"{ExecuteContext.Name}.{Name}.Applicaton"; }
            set { applicationProjectName = value; }
        }
        public string ApplicationShareProjectName
        {
            get { return applicationShareProjectName.IsNotNullOrWhiteSpaceBXJG() ? applicationShareProjectName : $"{ExecuteContext.Name}.{Name}.ApplicatonShare"; }
            set { applicationShareProjectName = value; }
        }
        public string BlazorHostProjectName
        {
            get { return blazorHostProjectName.IsNotNullOrWhiteSpaceBXJG() ? blazorHostProjectName : $"{ExecuteContext.Name}.{Name}.BlazorHost"; }
            set { blazorHostProjectName = value; }
        }
        public string ApiHostProjectName
        {
            get { return apiHostProjectName.IsNotNullOrWhiteSpaceBXJG() ? apiHostProjectName : $"{ExecuteContext.Name}.{Name}.ApiHost"; }
            set { apiHostProjectName = value; }
        }

    }
}

using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.Models.TokenAuth;

namespace ZLJ.Application.Common.Share.Auth
{
    public interface ITokenAuthAppService: IApplicationService
    {
        ///api/TokenAuth/Authenticate
       Task<  AuthenticateResultModel > Authenticate(AuthenticateModel model);
    }
}
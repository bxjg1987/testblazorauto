using BXJG.Common.Session;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common.Web.Session
{
    public class ReqSession : Common.Session.ISession
    {
        IHttpContextAccessor httpContextAccessor;

        public ReqSession(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public object Get(string key)
        {
            var r = httpContextAccessor.HttpContext.Request.GetFromQueryStringOrHeaderOrCookie(key);
            if (r.IsNullOrWhiteSpaceBXJG())
                r = httpContextAccessor.HttpContext.Session?.GetString(key);
            return r;
        }
    }

    //public class SessionSession : Common.Session.ISession {
    //    IHttpContextAccessor httpContextAccessor;

    //    public SessionSession(IHttpContextAccessor httpContextAccessor)
    //    {
    //        this.httpContextAccessor = httpContextAccessor;
    //    }

    //    public object Get(string key)
    //    {
    //        return httpContextAccessor.HttpContext.Request.GetFromQueryStringOrHeaderOrCookie(key);
    //    }
    //}
}

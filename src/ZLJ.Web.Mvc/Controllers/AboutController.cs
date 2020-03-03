using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using ZLJ.Controllers;

namespace ZLJ.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : ZLJControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}

using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using ZLJ.Controllers;

namespace ZLJ.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : ZLJControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}

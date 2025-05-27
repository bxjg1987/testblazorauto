using Abp.AspNetCore.Mvc.Controllers;
using Lazy.Captcha.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Web.Controllers
{
    /// <summary>
    /// 通用的验证码接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CaptchaController: AbpController
    {
        private readonly ICaptcha _captcha;

        public CaptchaController(ICaptcha captcha)
        {
            _captcha = captcha;
        }

        [HttpGet]
        public FileResult Captcha(string id)
        {
            var info = _captcha.Generate(id);
            // 有多处验证码且过期时间不一样，可传第二个参数覆盖默认配置。
            //var info = _captcha.Generate(id,120);
            var stream = new MemoryStream(info.Bytes);
            return File(stream, "image/gif");
        }
    }
}

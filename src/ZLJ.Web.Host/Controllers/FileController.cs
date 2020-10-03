using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using BXJG.Utils.File;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ZLJ.Controllers;
using ZLJ.Models;

namespace ZLJ.Controllers
{
    /*
     * 简单的文件上传功能，后期再详细设计
     * 目前考虑直接面向外网发布时使用Kestrel服务器。好像没有内置的文件的大小和类型限制
     * 简单的办法使用asp.net core的配置或选项系统来实现
     * 考虑多一点可能需要abp的settings或版本特征系统来完成
     * 目前只做以下限制：
     *      大小
     *      后缀（这种方式并不安全）
     *      必须为登录的用户
     */

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : ZLJControllerBase
    {
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly TempFileManager tempFileManager;

        public FileController(IConfiguration config, IWebHostEnvironment hostingEnvironment, TempFileManager tempFileManager)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.config = config;
            this.tempFileManager = tempFileManager;
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<IList<BXJG.Utils.File.FileResult>> UploadAsync(IFormFileCollection file, bool createThum = false)
        {
            var rts = new List<FileUploadResult>();
            var fs = file.Select(c => new Input(c.FileName, c.OpenReadStream(), c.ContentType));
            return await tempFileManager.UploadAsync(createThum, fs.ToArray());
            //foreach (var item in file)
            //{
            //    //此处需要对文件做各种检查  依赖abp的特征或设置系统

            //    //aaa.txt
            //    var hz = Path.GetExtension(item.FileName);//.txt
            //    var rdmName = Guid.NewGuid().ToString("N");//sdfsdfsdfsf3r2xsdf
            //    var fName = rdmName+hz;//sdfsdfsdfsf3r2xsdf.txt
            //    var fileName = Path.Combine(hostingEnvironment.WebRootPath, "upload/temp", fName);//e:\app\wwwroot\upload\sdfsdfsdfsf3r2xsdf.txt
            //    var xnlj = Path.Combine( "upload", fName);//upload\sdfsdfsdfsf3r2xsdf.txt
            //    using (var fs = System.IO.File.Create(fileName))
            //    {
            //        await item.CopyToAsync(fs, HttpContext.RequestAborted);
            //    }
            //    rts.Add(new FileUploadResult { RelativePath= xnlj });
            //}
            //return rts;
        }
    }
}
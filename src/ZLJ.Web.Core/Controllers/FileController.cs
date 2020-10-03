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
        private readonly TempFileManager tempFileManager;

        public FileController(TempFileManager tempFileManager)
        {
            this.tempFileManager = tempFileManager;
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<List<(string FileRelativePath, string ThumRelativePath)>> UploadAsync(IFormFileCollection file, [FromHeader]bool createThum = false)
        {
            var rts = new List<FileUploadResult>();
            var fs = file.Select(c => new Input(c.FileName, c.OpenReadStream(), c.ContentType));
            var r = await tempFileManager.UploadAsync(createThum, fs.ToArray());
            return r.Select(c => (c.FileRelativePath, c.ThumRelativePath)).ToList();
        }
    }
}
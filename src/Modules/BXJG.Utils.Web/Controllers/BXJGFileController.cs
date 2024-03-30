using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.Authorization;
using BXJG.Utils.Application.Share.Files;
using BXJG.Utils.Files;
using BXJG.Utils.Share;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace BXJG.Utils.Web.Controllers
{
    /*
     * 目前只做以下限制：
     *      大小
     *      后缀（这种方式并不安全）
     *      必须为登录的用户
     */

    /// <summary>
    /// 通用的文件管理接口
    /// 上传、下载、删除等
    /// 目前仅考虑操作权限，不考虑数据权限
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AbpAuthorize]
    public class BXJGFileController : AbpController
    {
        private readonly FileManager tempFileManager;

        public BXJGFileController(FileManager tempFileManager)
        {
            base.LocalizationSourceName = BXJGUtilsConsts.LocalizationSourceName;
            this.tempFileManager = tempFileManager;
        }

        //图片均生成缩略图，考虑到pc和移动端，目前自动生成两种尺寸的缩略图
        //尺寸大小直接硬编码，因为目前市面上的大屏 小屏就那几种

        /// <summary>
        /// 上传一个或多个文件，仅存储到临时目录
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FileDto> UploadAsync(IFormFile file)
        {
            // var rts = new List<FileUploadResult>();
            //var fs = file.Select(c => new FileInput(c.FileName, c.OpenReadStream()));
            using var fs = file.OpenReadStream();

            var r = await tempFileManager.UploadToTemp(fs);

            return new FileDto { Name = file.FileName, Path = r };
            //   return ObjectMapper.Map<List<FileDto>>(r);
            //return r.Select(c => new FileDto
            //{
            //    FilePath = c.FileRelativePath,
            //    ThumPath = c.ThumRelativePath,
            //    FileUrl = c.FileUrl,
            //    ThumUrl = c.ThumUrl
            //}).ToList();
        }
    }
}
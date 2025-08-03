using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.Auditing;
using Abp.Authorization;
using BXJG.Utils.Application.Share.Files;
using BXJG.Utils.Files;
using BXJG.Utils.Share;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
    public class BXJGFileController : AbpController
    {
        private readonly Lazy<FileManager> tempFileManager;
        private readonly Lazy<FileDownloader> fileDownloader;

        public BXJGFileController(Lazy<FileManager> tempFileManager, Lazy<FileDownloader> fileDownloader)
        {
            base.LocalizationSourceName = BXJGUtilsConsts.LocalizationSourceName;
            this.tempFileManager = tempFileManager;
            this.fileDownloader = fileDownloader;
        }

        //图片均生成缩略图，考虑到pc和移动端，目前自动生成两种尺寸的缩略图
        //尺寸大小直接硬编码，因为目前市面上的大屏 小屏就那几种

        /// <summary>
        /// 上传一个或多个文件，仅存储到临时目录
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [EnableRateLimiting("upload_file")]
        public async Task<FileDto> UploadAsync(IFormFile file)
        {
            // var rts = new List<FileUploadResult>();
            //var fs = file.Select(c => new FileInput(c.FileName, c.OpenReadStream()));
            using var fs = file.OpenReadStream();

            var r = await tempFileManager.Value.UploadToTemp(fs);

            return new FileDto { Name = file.FileName, Path = r };
            //return ObjectMapper.Map<List<FileDto>>(r);
            //return r.Select(c => new FileDto
            //{
            //    FilePath = c.FileRelativePath,
            //    ThumPath = c.ThumRelativePath,
            //    FileUrl = c.FileUrl,
            //    ThumUrl = c.ThumUrl
            //}).ToList();
        }

        //此逻辑比较简单，下载文件必须要controller，若把controller看成是应用服务平级的话
        //如此理解，文件下载的权限判断逻辑也没必要再封装个应用服务了

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="id">文件id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<PhysicalFileResult> Download(Guid id)
        {
            var r = await this.fileDownloader.Value.GetAbsolutePath(id);

            if (r.Permission == Share.Files.FilePermission.Further)
                throw new AbpAuthorizationException("请使用具体业务独立的文件访问接口");

            if (r.Permission == Share.Files.FilePermission.Authenticated && !base.AbpSession.UserId.HasValue)
                throw new AuthenticationFailureException("请登录");

            return PhysicalFile(r.RelativePath, r.ResponseContentType, r.RealFullName);
        }

        /// <summary>
        /// 获取文件缩略图
        /// </summary>
        /// <param name="id">文件id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [DisableAuditing]
        public async Task<PhysicalFileResult> DownloadThum(Guid id)
        {
            var r = await this.fileDownloader.Value.GetAbsolutePath(id);

            if (r.Permission == Share.Files.FilePermission.Further)
                throw new AbpAuthorizationException("请使用具体业务独立的文件访问接口");

            if (r.Permission == Share.Files.FilePermission.Authenticated && !base.AbpSession.UserId.HasValue)
                throw new AuthenticationFailureException("请登录");

            return PhysicalFile(r.RelativePathThumbnail, "image/jpeg", r.RealFullName);
        }
    }
}
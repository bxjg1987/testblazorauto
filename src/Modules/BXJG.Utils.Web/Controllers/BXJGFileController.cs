using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using BXJG.Utils.Application.Share.Files;
using BXJG.Utils.Extensions;
using BXJG.Utils.Files;
using BXJG.Utils.Share;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BXJG.Utils.Web.Controllers
{
    /*
     * 目前只做以下限制：
     *      大小
     *      后缀（这种方式并不安全）
     *      必须为登录的用户
     *      
     * 附件的设置必须是事务性的，所以不能放这里的通用接口中
     */

    /// <summary>
    /// 通用的文件/附件管理接口
    /// 上传、下载、删除等
    /// 目前仅考虑操作权限，不考虑数据权限
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BXJGFileController : AbpController
    {
        private readonly Lazy<FileManager> fileManager;
        private readonly Lazy<FileDownloader> fileDownloader;
        private readonly Lazy<IRepository<AttachmentEntity, Guid>> attachmentRepository;

        public BXJGFileController(Lazy<FileManager> tempFileManager, Lazy<FileDownloader> fileDownloader, Lazy<IRepository<AttachmentEntity, Guid>> attachmentRepository)
        {
            base.LocalizationSourceName = BXJGUtilsConsts.LocalizationSourceName;
            this.fileManager = tempFileManager;
            this.fileDownloader = fileDownloader;
            this.attachmentRepository = attachmentRepository;
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
        public async Task<UploadTempFileDto> UploadAsync(IFormFile file)
        {
            // var rts = new List<FileUploadResult>();
            //var fs = file.Select(c => new FileInput(c.FileName, c.OpenReadStream()));
            using var fs = file.OpenReadStream();

            var r = await fileManager.Value.UploadToTemp(fs);

            return new UploadTempFileDto { Name = file.FileName, Path = r };
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
        [UnitOfWork(false)]
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
        [UnitOfWork(false)]
        public async Task<PhysicalFileResult> DownloadThum(Guid id)
        {
            var r = await this.fileDownloader.Value.GetAbsolutePath(id);

            if (r.Permission == Share.Files.FilePermission.Further)
                throw new AbpAuthorizationException("请使用具体业务独立的文件访问接口");

            if (r.Permission == Share.Files.FilePermission.Authenticated && !base.AbpSession.UserId.HasValue)
                throw new AuthenticationFailureException("请登录");

            return PhysicalFile(r.RelativePathThumbnail, "image/jpeg", r.RealFullName);
        }


        /// <summary>
        /// 通用的，获取匿名和登录用户的附件关联的文件列表，
        /// 仅获取指定实体id的数据的单个属性的附件列表
        /// 
        /// <para>
        /// 需要严格权限控制的，最好在具体实体对应的应用服务查询中去处理
        /// 不过在最终下载时还会验证权限的，所以这里直接允许获取也无所谓
        /// </para>
        /// 
        /// <para>应用场景：后台管理或前台希望获取某个实体的某个属性下的附件文件列表</para>
        /// 
        /// <para>
        /// 若确定文件id是整个系统全局唯一的，如：guid，则实体类型可省略，属性名必须给 
        /// 因为后期可能加属性，所以建议总是提供属性名。
        /// 总的来说尽可能提供全更好
        /// </para>
        /// 
        /// </summary>
        /// <param name="entityType">实体类的完整名称</param>
        /// <param name="propertyName">可选的属性名</param>
        /// <param name="entityId">实体id</param>
        /// <returns></returns>
        [HttpGet]//必须加，否则swagger报错
        [UnitOfWork(false)]
        public async Task<List<FileDto>> GetFiles([Required] string entityId, string? entityType = default, string? propertyName = default)
        {
            var list = await (await attachmentRepository.Value.GetAllAsync()).WhereAttachment(entityType, propertyName, entityIds: entityId)
                                                                             .Where(x => x.File.Permission == Share.Files.FilePermission.Anonymous || (AbpSession.UserId.HasValue && x.File.Permission == Share.Files.FilePermission.Authenticated))
                                                                             .OrderBy(x => x.OrderIndex)
                                                                             .Select(x => x.File)
                                                                             .ToArrayAsync(HttpContext.RequestAborted);
            //foreach (var r in list)
            //{
            //    if (r.Permission == Share.Files.FilePermission.Further)
            //        throw new AbpAuthorizationException("请使用具体业务独立的文件访问接口");

            //    if (r.Permission == Share.Files.FilePermission.Authenticated && !base.AbpSession.UserId.HasValue)
            //        throw new AuthenticationFailureException("请登录");
            //}
            return ObjectMapper.Map<List<FileDto>>(list);
        }
        /// <summary>
        /// 获取一个实体对象的多个属性的附件列表
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityType"></param>
        /// <returns>key属性名；value附件列表</returns>
        [HttpGet]//必须加，否则swagger报错
        [UnitOfWork(false)]
        public async Task<List<AttachmentDto>> GetAttachments([Required] string entityId, string? entityType = default)
        {
            var list = await (await attachmentRepository.Value.GetAllAsync()).WhereAttachment(entityType, entityIds: entityId)
                                                                             .Where(x => x.File.Permission == Share.Files.FilePermission.Anonymous || (AbpSession.UserId.HasValue && x.File.Permission == Share.Files.FilePermission.Authenticated))
                                                                             //.OrderBy(x => x.OrderIndex)
                                                                             .ToArrayAsync(HttpContext.RequestAborted);
            var dtos = new List<AttachmentDto>();
            foreach (var item in list.GroupBy(c => c.PropertyName))
            {
                var files = item.OrderBy(x => x.OrderIndex).Select(x => x.File).ToList();

                var attachment = list.First(d => d.PropertyName == item.Key);
                attachment.File = default;

                var attdto = ObjectMapper.Map<AttachmentDto>(attachment);
                var fileDtos = ObjectMapper.Map<List<FileDto>>(files);
                attdto.Files = fileDtos;

            }
            return dtos;
        }

        ///// <summary>
        ///// 通用的，获取匿名和登录用户的附件列表
        ///// 强权限控制的，最好在具体实体对应的应用服务查询中去处理
        ///// 
        ///// <para>应用场景：后台管理或前台希望获取某个实体的某个属性下的附件文件列表</para>
        ///// 
        ///// <para>应用场景：后台管理或前台希望获取某个实体的某个属性下的附件文件列表</para>
        ///// </summary>

        ///// <param name="entityType">实体类的完整名称</param>
        ///// <param name="propertyName">可选的属性名</param>
        ///// <param name="entityIds">实体id集合</param>
        ///// <returns></returns>
        //[HttpPost]///必须加，否则swagger报错
        //public async Task<List<AttachmentDto>> GetAttachments([Required] string entityType, string? propertyName = default, params string[] entityIds)
        //{
        //    var q = await attachmentRepository.Value.GetAllAsync();
        //    var list = await q.WhereAttachment(entityType, propertyName, false, entityIds).Include(x => x.File).OrderBy(x => x.OrderIndex).ToListAsync(base.HttpContext.RequestAborted);

        //    foreach (var r in list)
        //    {
        //        if (r.File.Permission == Share.Files.FilePermission.Further)
        //            throw new AbpAuthorizationException("请使用具体业务独立的文件访问接口");

        //        if (r.File.Permission == Share.Files.FilePermission.Authenticated && !base.AbpSession.UserId.HasValue)
        //            throw new AuthenticationFailureException("请登录");
        //    }

        //    return base.ObjectMapper.Map<List<AttachmentDto>>(list);
        //}
    }
}
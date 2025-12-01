using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.File
{
    public class AttachmentAppService //: ITransientDependency
    {
        private readonly AttachmentManager attachmentManager;
        private readonly TempFileManager tempFileManager;
        private readonly IRepository<AttachmentEntity, Guid> repository;
        private readonly string entityType;
        private readonly IObjectMapper objectMapper;

        public AttachmentAppService(AttachmentManager attachmentManager,
                                    TempFileManager tempFileManager,
                                    IRepository<AttachmentEntity, Guid> repository,
                                    string entityType, 
                                    IObjectMapper objectMapper)
        {
            this.attachmentManager = attachmentManager;
            this.tempFileManager = tempFileManager;
            this.repository = repository;
            this.entityType = entityType;
            this.objectMapper = objectMapper;
        }
        /// <summary>
        /// 设置附件
        /// </summary>
        /// <param name="entityId">实体id</param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<AttachmentDto>> SetAttachmentsAsync(object entityId, IList<AttachmentEditDto> input)
        {
            var items = await  attachmentManager.SetAttachmentsAsync(entityId, input.ToDictionary(c => c.FileUrl, c => c.ExtensionData as IDictionary<string, object>));
            return objectMapper.Map<List<AttachmentDto>>(items);
        }
        /// <summary>
        /// 获取附件
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns></returns>
        public async Task<List<AttachmentDto>> GetAttachmentsAsync(params object[] entityIds)
        {
            var items = await attachmentManager.GetAttachmentsAsync(entityIds);
            return objectMapper.Map<List<AttachmentDto>>(items);
        }
    }

    public class AttachmentAppService<TEntity> : AttachmentAppService
    {
        public AttachmentAppService(AttachmentManager attachmentManager,
                                    TempFileManager tempFileManager,
                                    IRepository<AttachmentEntity, Guid> repository) : base(attachmentManager,
                                                                                        tempFileManager,
                                                                                        repository,
                                                                                        typeof(TEntity).FullName)
        {
        }
    }
}

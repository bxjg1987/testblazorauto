using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using Abp.Linq.Extensions;
using System;
using Abp.UI;
using ZLJ.Authorization;
using Abp.Extensions;
using Abp.Organizations;
//using ZLJ.ABPFile;
//using ZLJ.Attachment;
using BXJG.Attachment;
using ZLJ.BaseInfo;
using BXJG.GeneralTree;

namespace ZLJ.Asset
{
    //附件的处理是否需要单独为EquipmentInfo定义一个领域服务？
    //讲道理是要，比如crud操作时要处理附件相关的内容
    //但是这样其它类似有附件的模块都要这么做，感觉定麻烦。so 2天再说吧

    public class EquipmentInfoAppService
        : AsyncCrudAppService<EquipmentInfoEntity,
                              EquipmentInfoDto,
                              long,
                              GetEquipmentInfoInput,
                              EquipmentInfoEditDto,
                              EquipmentInfoEditDto,
                              EntityDto<long>,
                              EntityDto<long>>,
        IEquipmentInfoAppService
    {
        private readonly GeneralTreeManager organizationUnitManager;
        private readonly IRepository<GeneralTreeEntity, long> dicRepository;

        private readonly IEquipmentInfoRepository ownerRepository;

        public EquipmentInfoAppService(
            IEquipmentInfoRepository repository,
            GeneralTreeManager organizationUnitManager,
             IRepository<GeneralTreeEntity, long> dicRepository
            ) : base(repository)
        {
            this.ownerRepository = repository;
            this.dicRepository = dicRepository;
            this.organizationUnitManager = organizationUnitManager;
            GetAllPermissionName = PermissionNames.AdministratorAssetEquipmentInfo;
            GetPermissionName = PermissionNames.AdministratorAssetEquipmentInfo;
            CreatePermissionName = PermissionNames.AdministratorAssetEquipmentInfoCreate;
            UpdatePermissionName = PermissionNames.AdministratorAssetEquipmentInfoUpdate;
            DeletePermissionName = PermissionNames.AdministratorAssetEquipmentInfoDelete;
        }
        public async Task<List<long>> DeleteBatchAsync(long[] input)
        {
            CheckDeletePermission();

            var ids = new List<long>();
            foreach (var item in input)
            {
                try
                {
                    //await attachmentManager.SetAttachmentAsync(ABPConsts.AttachmentModuleName, item, null);
                    await Repository.DeleteAsync(item);

                    ids.Add(item);
                }
                catch { }
            }
            return ids;
        }

        public async Task<List<EquipmentInfoDto>> GetEnclosureAsync(double latitude, double longitude, double distance)
        {
            base.CheckGetPermission();
            var list = await ownerRepository.GetEnclosureAsync(latitude, longitude, distance);
            return base.ObjectMapper.Map<List<EquipmentInfoDto>>(list);
        }

        protected override IQueryable<EquipmentInfoEntity> CreateFilteredQuery(GetEquipmentInfoInput input)
        {
            var q = Repository.GetAllIncluding(c => c.Area);

            if (!input.AreaCode.IsNullOrWhiteSpace())
            {
                q = q.Where(c => c.Area.Code.StartsWith(input.AreaCode));
            }
            //else if (input.AreaId.HasValue && input.AreaId.Value != 0)
            //{
            //    //后面使用join进行优化
            //    //var code = organizationUnitManager.GetCode(input.CategoryId.Value);
            //    //q = q.Where(c => c.Category.Code.StartsWith(code));
            //    q = q.Where(c => c.Category.Code.StartsWith(
            //          dicRepository.GetAll().Where(d => d.Id == input.AreaId).Select(d => d.Code).Single()
            //          ));
            //}

            q = q.WhereIf(!input.Keywords.IsNullOrWhiteSpace(), c =>
                  c.Code.Contains(input.Keywords) ||
                  c.Size.Contains(input.Keywords));

            return q;
        }

        //public override async Task<EquipmentInfoDto> GetAsync(EntityDto<long> input)
        //{
        //    var dto = await base.GetAsync(input);
        //    var fj = await attachmentManager.GetAttachmentAsync(ABPConsts.AttachmentModuleName, dto.Id);
        //    dto.Attachments = AttachmentToFileDtos(fj);
        //    return dto;
        //}

        //public override async Task<EquipmentInfoDto> CreateAsync(EquipmentInfoEditDto input)
        //{
        //    var dto = await base.CreateAsync(input);
        //    var fj = await attachmentManager.SetAttachmentAsync(ABPConsts.AttachmentModuleName, dto.Id, input.Attachments.ToArray());
        //    dto.Attachments = AttachmentToFileDtos(fj);
        //    return dto;
        //}

        //public override async Task DeleteAsync(EntityDto<long> input)
        //{
        //    CheckDeletePermission();

        //    await attachmentManager.SetAttachmentAsync(ABPConsts.AttachmentModuleName, input.Id, null);
        //    await Repository.DeleteAsync(input.Id);
        //}

        //public override async Task<EquipmentInfoDto> UpdateAsync(EquipmentInfoEditDto input)
        //{
        //    var dto = await base.UpdateAsync(input);
        //    var fj = await attachmentManager.SetAttachmentAsync(ABPConsts.AttachmentModuleName, dto.Id, input.Attachments.ToArray());
        //    dto.Attachments = AttachmentToFileDtos(fj);

        //    return dto;
        //}
        //强烈建议这样的转换使用AutoMapper，因为AttachmentEntity>FileDto的转换很多地方都需要
        //protected virtual IReadOnlyList<BXJGAttachmentDto> AttachmentToFileDtos(IEnumerable<BXJGAttachmentEntity> att)
        //{
        //    return att.Select(c => new BXJGAttachmentDto
        //    {
        //        AttachmentId = c.Id,
        //        Id = c.FileId,
        //        Extension = c.File.GetExtension(),
        //        Keywords = c.File.Keywords,
        //        MD5 = c.File.MD5,
        //        MnemonicCode = c.File.MnemonicCode,
        //        Name = c.File.Name,
        //        RelativePath = c.File.GetRelativePath(),
        //        Size = c.File.Size,
        //        Mime = c.File.GetMime()
        //    }).ToList();
        //}
    }
}

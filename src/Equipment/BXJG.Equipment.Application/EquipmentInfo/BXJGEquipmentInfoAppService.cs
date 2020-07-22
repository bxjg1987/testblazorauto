using Abp.Application.Services;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXJG.Equipment.EquipmentInfo
{
    public abstract class BXJGEquipmentInfoAppService<TEntity> : AsyncCrudAppService<TEntity, EquipmentInfoDto, long, EquipmentInfoGetAllInput, EquipmentInfoEditDto>, IBXJGEquipmentInfoAppService
        where TEntity : Entity<long>
    {
        public BXJGEquipmentInfoAppService(IRepository<TEntity, long> repository) : base(repository)
        {
        }

        //public Task DeleteBatch(long[] ids)
        //{
        //    return base.Repository.DeleteAsync(c => ids.Contains(c.Id));
        //}

        //protected override IQueryable<EquipmentInfoEntity<TDataDictionary>> CreateFilteredQuery(EquipmentInfoGetAllInput input)
        //{
        //    return base.CreateFilteredQuery(input).Include(c => c.ZZMM);
        //}
    }
    /// <summary>
    /// 设备信息定义的应用服务
    /// 关联的设备信息定义的实体类型为EquipmentInfoEntity，它继承EquipmentInfoEntity<GeneralTreeEntity>。若模块调用方要使用自己的数据字典类型
    /// 则需要自定义实体和应用服务，且都有相关的父类
    /// </summary>
    public class BXJGEquipmentInfoAppService : BXJGEquipmentInfoAppService<EquipmentInfoEntity>
    {
        public BXJGEquipmentInfoAppService(IRepository<EquipmentInfoEntity, long> repository) : base(repository)
        {
        }
    }
}

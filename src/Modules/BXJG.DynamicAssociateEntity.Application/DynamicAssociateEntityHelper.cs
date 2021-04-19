using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Dynamic;
using Abp.UI;
using Abp.Extensions;
using Abp.Dependency;

namespace BXJG.DynamicAssociateEntity
{
    public class DynamicAssociateEntityHelper
    {
        protected readonly DynamicAssociateEntityDefineManager dynamicAssociateEntityDefineManager;
        protected readonly DefineMapGroup defines;
        public DynamicAssociateEntityHelper(DynamicAssociateEntityDefineManager dynamicAssociateEntityDefineManager, string groupName)
        {
            this.dynamicAssociateEntityDefineManager = dynamicAssociateEntityDefineManager;
            defines = dynamicAssociateEntityDefineManager.GroupedDefines[groupName];
        }

        public List<List<KeyValuePair<string, object>>> DtoMapToEntity(IDictionary<string, object> dto)
        {
            return dto.MapToEntity(defines.TopFlatItems);
        }
        public string DtoMapToEntityJsonString(IDictionary<string, object> dto)
        {
            return dto.DtoMapToEntityJsonString(defines.TopFlatItems);
        }
        /// <summary>
        /// 将用户提交的动态关联外键的数据转换映射到实体的对应属性上，最终以json格式保存，且保留级联结构
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="entity"></param>
        public void DtoMapToEntity(IDynamicAssociateEditDto dto, IDynamicAssociateEntity entity)
        {
            entity.DynamicAssociateData = DtoMapToEntityJsonString(dto.DynamicAssociateData);
        }

        public IDynamicAssociateEntityService GetService(string name)
        {
            return dynamicAssociateEntityDefineManager.GetService<IDynamicAssociateEntityService>(name);
        }
    }
}
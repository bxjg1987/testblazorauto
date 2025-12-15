using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.UI;
using BXJG.Utils.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Domain.Entities
{
    public static class AbpEntityExt
    {
        /// <summary>
        /// 检查实体是否启用，如果未启用则抛出异常。
        /// </summary>
        /// <param name="passivable"></param>
        /// <param name="entityTypeName">业务名称</param>
        /// <param name="id">唯一id</param>
        public static void Check(this IPassivable passivable, string entityTypeName=default, object? id = default, ILocalizationSource? ls = default)
        {
            if (passivable.IsActive == false)
            {
                if (id == null)
                    id = (passivable as IEntity)?.Id;
                if (id == null)
                    id = (passivable as IEntityDto)?.Id;
                if (ls == null)
                    ls = LocalizationHelper.Manager.GetSource(BXJGUtilsConsts.LocalizationSourceName);
         
                    entityTypeName = ls.GetString(entityTypeName??passivable.GetType().FullName);
                UserFriendlyExceptionFactory.Throw(BXJGUtilsConsts.EntityIsNotActive, ls, entityTypeName, id);
            }
        }
    }
}

using Abp.Domain.Uow;
using Abp.EntityHistory;
using BXJG.Utils.Share;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.EFCore.EntityHistory
{
    /// <summary>
    /// abp实例提示记录的核心逻辑在EntityHistoryHelper中
    /// 此类替换并扩展它
    /// </summary>
    public class BXJGEntityHistoryHelper : EntityHistoryHelper
    {
        public BXJGEntityHistoryHelper(IEntityHistoryConfiguration configuration, IUnitOfWorkManager unitOfWorkManager) : base(configuration, unitOfWorkManager)
        {
        }

        protected override bool? ShouldSaveEntityHistory(EntityEntry entityEntry)
        {
            var r = base.ShouldSaveEntityHistory(entityEntry);

            if (r.HasValue && r.Value)
            {
                if (UnitOfWorkManager.Current.Items.TryGetValue(BXJGUtilsConsts.ExcludeEntities, out var pp))
                {
                    var p = pp as Dictionary<Type, HashSet<string>>;
                    if (p.TryGetValue(entityEntry.Entity.GetType(), out var jh))
                    {
                        if (jh.Contains(GetEntityId(entityEntry)))
                            return false;
                    }
                }
            }

            return r;
        }
    }
}

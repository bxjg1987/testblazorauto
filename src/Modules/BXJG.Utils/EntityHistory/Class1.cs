using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Uow;
using BXJG.Utils;

namespace Abp.Domain.Uow
{
    public static class Class1
    {
        public static IActiveUnitOfWork SetExcludeEntity<TEntity>(this IActiveUnitOfWork uow, object id)
        {
           return uow.SetExcludeEntity(typeof(TEntity), id);
        }
        public static IActiveUnitOfWork SetExcludeEntity(this IActiveUnitOfWork uow, Type t, object id)
        {
            var strId = id.ToString();
            Dictionary<Type, HashSet<string>> x;

            if (!uow.Items.TryGetValue(BXJGUtilsConsts.ExcludeEntities, out var x1))
            {
                x = new Dictionary<Type, HashSet<string>>();
                uow.Items[BXJGUtilsConsts.ExcludeEntities] = x;
            }
            else
                x = x1 as Dictionary<Type, HashSet<string>>;

            HashSet<string> ids;
            if (!x.TryGetValue(t, out ids))
            {
                ids = new HashSet<string>();
                x[t] = ids;
            }
            ids.Remove(strId);
            ids.Add(strId);
            return uow;
        }
        public static IActiveUnitOfWork RemoveExcludeEntity<TEntity>(this IActiveUnitOfWork uow,  object id)
        {
            return uow.RemoveExcludeEntity(typeof(TEntity), id);
        }
        public static IActiveUnitOfWork RemoveExcludeEntity(this IActiveUnitOfWork uow, Type t, object id)
        {
            var strId = id.ToString();
            Dictionary<Type, HashSet<string>> x;

            if (!uow.Items.TryGetValue(BXJGUtilsConsts.ExcludeEntities, out var x1))
                return uow;
            else
                x = x1 as Dictionary<Type, HashSet<string>>;

            HashSet<string> ids;
            if (!x.TryGetValue(t, out ids))
                return uow;

            ids.Remove(strId);
            return uow;
        }
    }
}

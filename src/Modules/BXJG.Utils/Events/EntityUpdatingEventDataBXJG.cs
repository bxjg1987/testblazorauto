using Abp.Events.Bus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Events
{
    public class EntityUpdatingEventDataBXJG<TEntity> : EntityUpdatingEventData<TEntity>
    {
        public object OriginalValue { get; protected set; }   
        public EntityUpdatingEventDataBXJG(TEntity entity,object obj) : base(entity)
        {
            this.OriginalValue = obj;
        }
    }
    public class EntityUpdatingEventDataBXJG<TEntity,TState> : EntityUpdatingEventDataBXJG<TEntity>
    {
        public EntityUpdatingEventDataBXJG(TEntity entity, object obj) : base(entity, obj)
        {
            this.OriginalValue = (TState) Convert.ChangeType(obj, typeof(TState));
        }

        public new TState OriginalValue { get; protected set; }
       
    }
}

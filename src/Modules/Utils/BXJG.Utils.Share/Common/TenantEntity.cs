using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Share
{
    public class TenantEntity : IMayHaveTenant, IEntity<object>
    {
        public object Id { get; set; }
        public int? TenantId { get; set; }

        public bool IsTransient()
        {
            return false;
        }
    }
    public class TenantEntity<TEntityKey> : TenantEntity, IEntity<TEntityKey>
    {
        public new TEntityKey Id
        {
            get
            {
                return (TEntityKey)(this as TenantEntity).Id;
            }
            set
            {
                (this as TenantEntity).Id = value;
            }
        }
    }
}

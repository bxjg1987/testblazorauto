using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Authorization.Roles;

namespace ZLJ.Core.BaseInfo.Post
{
    public class PostEntity : Role
    {
        ///  public string t { get; set; }
        public PostEntity()
        {
        }

        public PostEntity(int? tenantId, string displayName) : base(tenantId, displayName)
        {
        }

        public PostEntity(int? tenantId, string name, string displayName) : base(tenantId, name, displayName)
        {
        }
    }
}

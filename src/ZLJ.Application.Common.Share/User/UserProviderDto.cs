using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.OU;
using ZLJ.Application.Common.Share.Roles;

namespace ZLJ.Application.Common.Share.User
{
    public class UserProviderDto : BXJG.Utils.Application.Share.User.UserSelectDto
    {
        //public  new IEnumerable<OUSelectDto> Ous { get; set; }
        //public new IEnumerable<RoleForSelectDto> Roles { get; set; }
        //[JsonConverter(typeof(JsonConverter<OUSelectDto>))]
        // public new IEnumerable<OUSelectDto> Ous { get; set; }
        [JsonConverter(typeof(OusConverter<OUSelectDto>))]
        public override IEnumerable<IGeneralTree> Ous { get => base.Ous; set => base.Ous = value; }
    }
}

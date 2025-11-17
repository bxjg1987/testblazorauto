using Abp.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.BaseInfo.Post;
using ZLJ.Core.BaseInfo.StaffInfo;
using ZLJ.Core.OU;

namespace ZLJ.Application.Common.StaffInfo
{
    public class QueryTemp
    {
        public StaffInfoEntity Staff { get; set; }
        public OrganizationUnitEntity Ou { get; set; }
        public PostEntity Post { get; set; }
    }
}

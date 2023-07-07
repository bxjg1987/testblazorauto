using Abp.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.BaseInfo;
using ZLJ.BaseInfo.Post;
using ZLJ.BaseInfo.StaffInfo;

namespace ZLJ.App.Common.StaffInfo
{
    public class QueryTemp
    {
        public StaffInfoEntity Staff { get; set; }
        public OrganizationUnitEntity Ou { get; set; }
        public PostEntity Post { get; set; }
    }
}

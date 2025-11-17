using Abp.Organizations;
using BXJG.Utils.OU;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Core.Customer
{
    public class CustomerOUManager : BXJGOrganizationUnitManager
    {
        public CustomerOUManager(IRepository<OrganizationUnit, long> organizationUnitRepository) : base(organizationUnitRepository)
        {
        }

        protected override Task ValidateOrganizationUnitAsync(OrganizationUnit organizationUnit)
        {
            //return base.ValidateOrganizationUnitAsync(organizationUnit);
            return Task.CompletedTask;
        }

        protected override void ValidateOrganizationUnit(OrganizationUnit organizationUnit)
        {
            //base.ValidateOrganizationUnit(organizationUnit);
        }
    }
}

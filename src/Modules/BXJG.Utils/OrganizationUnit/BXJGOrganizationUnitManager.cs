using Abp.Domain.Repositories;
using Abp.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.OrganizationUnit
{
    public class BXJGOrganizationUnitManager : OrganizationUnitManager
    {
        public BXJGOrganizationUnitManager(IRepository<Abp.Organizations.OrganizationUnit, long> organizationUnitRepository) : base(organizationUnitRepository)
        {
        }
    }
}

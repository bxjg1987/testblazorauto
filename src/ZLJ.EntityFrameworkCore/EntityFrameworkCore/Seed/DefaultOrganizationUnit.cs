using Abp.Organizations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLJ.BaseInfo;

namespace ZLJ.EntityFrameworkCore.Seed
{
    public class DefaultOrganizationUnit
    {
        private readonly ZLJDbContext _context;
        int tenantId;
        public DefaultOrganizationUnit(ZLJDbContext context, int tenantId)
        {
            _context = context;
            this.tenantId = tenantId;
        }

        public void Create()
        {
            var n = "XXX有限公司";
            if (_context.OrganizationUnits.IgnoreQueryFilters().Any(ef => ef.TenantId == tenantId && ef.DisplayName == n))
            {
                return;
            }
            var o = new OrganizationUnitEntity(tenantId, n);
            o.Code = OrganizationUnit.CreateCode(1);
            o.OUType = Enums.OUType.HeadOffice;
            _context.OrganizationUnits.Add(o);
            _context.SaveChanges();
        }

        //private void CreateEditions()
        //{
        //    var defaultEdition = _context.Editions.IgnoreQueryFilters().FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
        //    if (defaultEdition == null)
        //    {
        //        defaultEdition = new Edition { Name = EditionManager.DefaultEditionName, DisplayName = EditionManager.DefaultEditionName };
        //        _context.Editions.Add(defaultEdition);
        //        _context.SaveChanges();

        //        /* Add desired features to the standard edition, if wanted... */
        //    }
        //}

        //private void CreateIfNotExists(int? tenantId, string displayName, long? parentId = null)
        //{
        //    if (_context.OrganizationUnits.IgnoreQueryFilters().Any(ef => ef.TenantId == tenantId && ef.DisplayName == displayName))
        //    {
        //        return;
        //    }



        //    var o = new OrganizationUnit(tenantId, displayName, parentId);

        //    if (parentId.HasValue) {
        //        var parent = _context.OrganizationUnits.Include(c => c.Children).Single(c => c.Id == parentId.Value);

        //        var list = 
        //        //OrganizationUnit
        //    }
        //    else
        //        o.Code=   OrganizationUnit.MaxDisplayNameLength


        //    _context.EditionFeatureSettings.Add(new EditionFeatureSetting
        //    {
        //        Name = featureName,
        //        Value = isEnabled.ToString(),
        //        EditionId = editionId
        //    });
        //    _context.SaveChanges();
        //}
    }
}

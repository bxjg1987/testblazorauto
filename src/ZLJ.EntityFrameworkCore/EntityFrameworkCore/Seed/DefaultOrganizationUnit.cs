using Abp.Organizations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLJ.BaseInfo;
using ZLJ.Core.Share;

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
            var n = "重庆元凯办公";
            if (_context.OrganizationUnits.IgnoreQueryFilters().Any(ef => ef.TenantId == tenantId && ef.DisplayName == n))
            {
                return;
            }
            var o = new OrganizationUnitEntity(tenantId, n);
            o.Code = OrganizationUnit.CreateCode(1);
            o.OUType = OUType.HeadOffice;
            o.Children = new List<OrganizationUnit>();

            o.Children.Add(new OrganizationUnitEntity(tenantId, "销售部")
            {
                 Code = OrganizationUnit.CreateCode(1,1),
                OUType = OUType.Department
            });
            o.Children.Add(new OrganizationUnitEntity(tenantId, "采购部")
            {
                Code = OrganizationUnit.CreateCode(1, 2),
                OUType = OUType.Department
            });
            o.Children.Add(new OrganizationUnitEntity(tenantId, "仓储")
            {
                Code = OrganizationUnit.CreateCode(1, 3),
                OUType = OUType.Department
            });
            o.Children.Add(new OrganizationUnitEntity(tenantId, "技术部")
            {
                Code = OrganizationUnit.CreateCode(1, 4),
                OUType = OUType.Department
            });
            o.Children.Add(new OrganizationUnitEntity(tenantId, "财务部")
            {
                Code = OrganizationUnit.CreateCode(1, 5),
                OUType = OUType.Department
            });
            //o.Children = new List<OrganizationUnitEntity>
            //{

            //};
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

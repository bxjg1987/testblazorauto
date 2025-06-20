using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.BaseInfo.AssociatedCompany;

namespace System.Linq
{
    public static class AssociatedCompanyQureyableExt
    {
        //ef配置了默认自动include
        //public static IQueryable<AssociatedCompanyEntity> WithDetails(this IQueryable<AssociatedCompanyEntity> q) {
        //    return q.Include(x=>x.Level).Include(x=>x.Area);
        //}
    }
}

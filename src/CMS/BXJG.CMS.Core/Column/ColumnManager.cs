using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Column
{
    public class ColumnManager : GeneralTreeManager<ColumnEntity>
    {
        public ColumnManager(IRepository<ColumnEntity, long> repository) : base(repository)
        {
        }
    }
}

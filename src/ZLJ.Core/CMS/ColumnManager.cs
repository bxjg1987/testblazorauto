using Abp.Domain.Repositories;
using BXJG.CMS.Column;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZLJ.CMS
{
    public class ColumnManager : ColumnManager<GeneralTreeEntity>
    {
        public ColumnManager(IRepository<ColumnEntity<GeneralTreeEntity>, long> repository) : base(repository)
        {
        }
    }
}

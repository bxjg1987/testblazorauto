using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Column
{
    public class ColumnManager<TDataDictionary> : GeneralTreeManager<ColumnEntity<TDataDictionary>>
        where TDataDictionary: GeneralTreeEntity<TDataDictionary>
    {
        public ColumnManager(IRepository<ColumnEntity<TDataDictionary>, long> repository) : base(repository)
        {
        }
    }
}

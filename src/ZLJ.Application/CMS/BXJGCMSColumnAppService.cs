using Abp.Domain.Repositories;
using BXJG.CMS.Column;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZLJ.CMS
{
    /// <summary>
    /// 后台管理CMS栏目应用服务
    /// </summary>
    public class BXJGCMSColumnAppService : BXJGCMSColumnAppService<GeneralTreeEntity>
    {
        public BXJGCMSColumnAppService(IRepository<ColumnEntity<GeneralTreeEntity>, long> repository, ColumnManager manager) 
            : base(repository, manager)
        {
        }
    }
}

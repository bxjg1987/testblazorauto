using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZLJ.Administrative
{
    public class AdministrativeManager : GeneralTreeManager<AdministrativeEntity>
    {
        public AdministrativeManager(IRepository<AdministrativeEntity, long> repository) : base(repository)
        {
        }
    }
}

using Abp.Domain.Repositories;
using BXJG.Utils.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZLJ.Core.BaseInfo.Administrative
{
    /// <summary>
    /// 行政区领域服务
    /// </summary>
    public class AdministrativeManager : GeneralTreeManager<AdministrativeEntity>
    {
        public AdministrativeManager(IRepository<AdministrativeEntity, long> repository) : base(repository)
        {
        }
    }
}

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yitter.IdGenerator;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 单号生成器
    /// 基于雪花算法
    /// </summary>
    [Obsolete("用全局的吧")]
    public class OrderNoGenerator : Abp.Dependency.ISingletonDependency,IIdGenerator
    {
        IIdGenerator idGenerator;

        public OrderNoGenerator(BXJGWorkOrderConfig configuration)
        {
            var opt = new IdGeneratorOptions(configuration.NoWorkerId);
            idGenerator = new DefaultIdGenerator(opt);
        }

        public Action<OverCostActionArg> GenIdActionAsync { get => idGenerator.GenIdActionAsync; set => idGenerator.GenIdActionAsync = value; }

        public long NewLong()
        {
            return idGenerator.NewLong();
        }
    }
}

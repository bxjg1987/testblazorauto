using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.EFCore.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class AbpSettingsConfigurationSource : IConfigurationSource
    {
        Func<DbContext> dbContextFactory;
        ILoggerFactory loggerFactory;

        public AbpSettingsConfigurationSource(Func<DbContext> dbContextFactory, ILoggerFactory? loggerFactory=default)
        {
            this.dbContextFactory = dbContextFactory;
            this.loggerFactory = loggerFactory;
            //if (loggerFactory == default)
            //    loggerFactory = NullLoggerFactory.Instance;
        }

        //看这个唯一接口方法也能看出来，source是根据builder（内部是configurationManager）来构建对应的provider
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AbpSettingsConfigurationProvider(dbContextFactory, loggerFactory);
        }
    }
}

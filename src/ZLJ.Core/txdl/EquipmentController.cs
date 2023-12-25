using Abp.Dependency;
using Abp.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using txdl;
using System.Collections.Concurrent;
using System;

namespace ZLJ.txdl
{
    
    public interface IEquipmentController 
    {
        Task<IEnumerable<string>> UnOrLockAsync(bool isLock, params string[] ids);

        ValueTask ConfigChangedAsync(params string[] ids);

        Task<EquipmentBaseInfoSnapshot> InitGetAsync(string communicationType, IDictionary<string, object> communicationParameter);

        Task<IEnumerable<string>> GetCommunicationTypes();

        Task<IEnumerable<string>> CheckOnlineAsync(params string[] ids);
    }
}
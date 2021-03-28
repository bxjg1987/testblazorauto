

using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using BXJG.WorkOrder.Employee;
using BXJG.WorkOrder.Session;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLJ.Authorization.Users;

namespace ZLJ.WorkOrder
{
    public class EmployeeAppService : IEmployeeAppService
    {
        private readonly IRepository<User, long> userRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;
        public EmployeeAppService(IRepository<User, long> userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<IEnumerable<EmployeeDto>> GetByIdsAsync(params string[] ids)
        {
            var query = userRepository.GetAll()
                                      .Where(c => ids.Contains(c.Id.ToString()))
                                      .Select(c => new EmployeeDto
                                      {
                                          Id = c.Id.ToString(),
                                          Name = c.Name,
                                          Phone = c.PhoneNumber
                                      });
            return await AsyncQueryableExecuter.ToListAsync(query);
        }

        public async Task<IEnumerable<string>> GetIdsByKeywordAsync(string keyword)
        {
            var query = userRepository.GetAll()
                                      .WhereIf(!keyword.IsNullOrEmpty(), c => c.Name.Contains(keyword) || c.PhoneNumber.Contains(keyword))
                                      .Select(c => c.Id.ToString());
            return await AsyncQueryableExecuter.ToListAsync(query);
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync(string keyword)
        {
            var query = userRepository.GetAll()
                                      .WhereIf(!keyword.IsNullOrEmpty(), c => c.Name.Contains(keyword) || c.PhoneNumber.Contains(keyword))
                                      .Select(c => new EmployeeDto
                                      {
                                          Id = c.Id.ToString(),
                                          Name = c.Name,
                                          Phone = c.PhoneNumber
                                      });
            return await AsyncQueryableExecuter.ToListAsync(query);
        }
    }
    public class EmployeeSession : IEmployeeSession
    {
        private readonly IAbpSession abpSession;

        public EmployeeSession(IAbpSession abpSession)
        {
            this.abpSession = abpSession;
        }

        public string CurrentEmployeeId => abpSession.UserId?.ToString();
    }
}

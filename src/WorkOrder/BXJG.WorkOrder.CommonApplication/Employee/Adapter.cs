using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.Employee
{
    public class EmployeeAppService<TUser> : IEmployeeAppService where TUser : AbpUser<TUser>
    {
        private readonly IRepository<TUser, long> userRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;
        public EmployeeAppService(IRepository<TUser, long> userRepository)
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
}


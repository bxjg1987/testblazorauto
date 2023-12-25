using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Extensions;
using ZLJ.BaseInfo.StaffInfo;
using BXJG.WorkOrder.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.WorkOrder
{
    public class WorkOrderEmployeeService<TUser> : IEmployeeAppService where TUser : AbpUser<TUser>
    {
        IRepository<StaffInfoEntity, long> staffRepository;
        IRepository<TUser, long> userRepository;

        public WorkOrderEmployeeService(IRepository<StaffInfoEntity, long> staffRepository, IRepository<TUser, long> userRepository)
        {
            this.staffRepository = staffRepository;
            this.userRepository = userRepository;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync(string keyword)
        {
            var query = from c in staffRepository.GetAll()
                        join d in userRepository.GetAll() on c.UserId equals d.Id into g
                        from e in g.DefaultIfEmpty()
                        select new { c, e };
            if (!keyword.IsNullOrWhiteSpace())
                query = query.Where(c => c.c.Name.Contains(keyword) || c.e.PhoneNumber.Contains(keyword));

            var query1 = query.Select(c => new EmployeeDto
            {
                Id = c.c.Id.ToString(),
                Name = c.c.Name,
                Phone = c.e.PhoneNumber
            });

            return await query1.ToListAsync();
        }

        public async Task<IEnumerable<EmployeeDto>> GetByIdsAsync(params string[] ids)
        {
            var query = from c in staffRepository.GetAll()
                        join d in userRepository.GetAll() on c.UserId equals d.Id into g
                        from e in g.DefaultIfEmpty()
                        select new { c, e };

            query = query.Where(c => ids.Contains(c.c.Id.ToString()));

            var query1 = query.Select(c => new EmployeeDto
            {
                Id = c.c.Id.ToString(),
                Name = c.c.Name,
                Phone = c.e.PhoneNumber
            });

            return await query1.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetIdsByKeywordAsync(string keyword)
        {
            var query = from c in staffRepository.GetAll()
                        join d in userRepository.GetAll() on c.UserId equals d.Id into g
                        from e in g.DefaultIfEmpty()
                        select new { c, e };
            if (!keyword.IsNullOrWhiteSpace())
                query = query.Where(c => c.c.Name.Contains(keyword) || c.e.PhoneNumber.Contains(keyword));

            var query1 = query.Select(c => c.c.Id.ToString());

            return await query1.ToListAsync();
        }
    }


}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VadgerWorkspace.Data.Entities;

namespace VadgerWorkspace.Data.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>
    {
        public EmployeeRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Employee> GetEmployeeByIdAsync(long Id)
        {
            return await FindByCondition(employee => employee.Id == Id).FirstOrDefaultAsync();
        }

        public Employee GetEmployeeByIdSync(long id)
        {
            return _dbContext.Set<Employee>().FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<Employee> GetAllAdmins()
        {
            return FindByCondition(employee => employee.IsAdmin == true);
        }

        public IEnumerable<Employee> GetAllGlobalAdmins()
        {
            return FindByCondition(employee => employee.IsAdmin == true && employee.IsLocalAdmin == false);
        }

        public IEnumerable<Employee> GetAllLocalsWithTown(string town)
        {
            return GetAllAdmins().Where(a => a.IsLocalAdmin == true && a.Town.Contains(town));
        }
    }
}

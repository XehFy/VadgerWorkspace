using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            return _dbContext.Set<Employee>().AsNoTracking().FirstOrDefault(o => o.Id == id);
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
            var arr = town.Split(' ');
            var emps = _dbContext.Set<Employee>().Where(x => x.IsLocalAdmin == true).AsEnumerable<Employee>();
            return emps.Where(a => a.Town.Split(' ')
                .Select(x => x).Intersect(arr).Any());

        }
        public IEnumerable<Employee> GetAllEmpsWithTown(string town)
        {
            var arr = town.Split(' ');
            var emps = _dbContext.Set<Employee>().Where(x => (x.IsLocalAdmin == true || x.IsAdmin==false )&& x.Town!= null && x.IsVerified == true).AsEnumerable<Employee>();
            return emps.Where(a => a.Town.Split(' ')
                .Select(x => x).Intersect(arr).Any());
        }
    }
}
